
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ST2_2026_Team_4_GameLibrary.Data;
using ST2_2026_Team_4_GameLibrary.Models;

namespace ST2_2026_Team_4_GameLibrary.Services
{
    public sealed class GameQueryExecutor
    {
        private readonly AppDbContext _context;

        public GameQueryExecutor(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Game>> ExecuteAsync(
            JSONQuery query,
            CancellationToken cancellationToken = default)
        {
            if (!global::ST2_2026_Team_4_GameLibrary.Services.GameQueryValidator
                .TryValidate(query, out string error))
            {
                throw new ArgumentException(error);
            }

            IQueryable<Game> games = _context.Games.AsNoTracking();

            if (query.Conditions.Count > 0)
            {
                var parameter = Expression.Parameter(typeof(Game), "g");
                Expression? combined = null;

                foreach (var condition in query.Conditions)
                {
                    var predicate = BuildCondition(parameter, condition);

                    combined = combined == null
                        ? predicate
                        : string.Equals(
                            query.LogicalOperator,
                            "OR",
                            StringComparison.OrdinalIgnoreCase)
                            ? Expression.OrElse(combined, predicate)
                            : Expression.AndAlso(combined, predicate);
                }

                var lambda = Expression.Lambda<Func<Game, bool>>(
                    combined!, parameter);

                games = games.Where(lambda);
            }

            bool descending = string.Equals(
                query.SortDirection,
                "DESC",
                StringComparison.OrdinalIgnoreCase);

            games = (query.OrderBy ?? "Title").ToLowerInvariant() switch
            {
                "id" => descending
                    ? games.OrderByDescending(g => g.Id)
                    : games.OrderBy(g => g.Id),

                "title" => descending
                    ? games.OrderByDescending(g => g.Title)
                    : games.OrderBy(g => g.Title),

                "genre" => descending
                    ? games.OrderByDescending(g => g.Genre)
                    : games.OrderBy(g => g.Genre),

                "developer" => descending
                    ? games.OrderByDescending(g => g.Developer)
                    : games.OrderBy(g => g.Developer),

                "releasedate" => descending
                    ? games.OrderByDescending(g => g.ReleaseDate)
                    : games.OrderBy(g => g.ReleaseDate),

                "description" => descending
                    ? games.OrderByDescending(g => g.Description)
                    : games.OrderBy(g => g.Description),

                _ => games.OrderBy(g => g.Title)
            };

            if (query.Limit.HasValue)
            {
                games = games.Take(query.Limit.Value);
            }

            return await games.ToListAsync(cancellationToken);
        }

        private static Expression BuildCondition(
            ParameterExpression parameter,
            QueryCondition condition)
        {
            string field = condition.Field.ToLowerInvariant();
            string op = condition.Operator.ToLowerInvariant();
            string value = condition.Value;

            Expression property = field switch
            {
                "id" => Expression.Property(parameter, nameof(Game.Id)),
                "title" => Expression.Property(parameter, nameof(Game.Title)),
                "genre" => Expression.Property(parameter, nameof(Game.Genre)),
                "developer" => Expression.Property(parameter, nameof(Game.Developer)),
                "releasedate" => Expression.Property(parameter, nameof(Game.ReleaseDate)),
                "description" => Expression.Property(parameter, nameof(Game.Description)),
                _ => throw new ArgumentException("Неразрешено поле.")
            };

            if (field == "id")
            {
                if (!int.TryParse(value, out int id))
                {
                    throw new ArgumentException("Невалидна стойност за Id.");
                }

                return BuildComparison(property, op, Expression.Constant(id));
            }

            if (field == "releasedate")
            {
                if (!DateTime.TryParse(
                    value,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime date))
                {
                    throw new ArgumentException("Невалидна дата.");
                }

                return BuildComparison(property, op, Expression.Constant(date));
            }

            Expression stringProperty = field == "description"
                ? Expression.Coalesce(property, Expression.Constant(string.Empty))
                : property;

            var constant = Expression.Constant(value);

            return op switch
            {
                "equals" => Expression.Equal(stringProperty, constant),
                "notequals" => Expression.NotEqual(stringProperty, constant),

                "contains" => Expression.Call(
                    stringProperty,
                    typeof(string).GetMethod(
                        nameof(string.Contains),
                        new[] { typeof(string) })!,
                    constant),

                "startswith" => Expression.Call(
                    stringProperty,
                    typeof(string).GetMethod(
                        nameof(string.StartsWith),
                        new[] { typeof(string) })!,
                    constant),

                "endswith" => Expression.Call(
                    stringProperty,
                    typeof(string).GetMethod(
                        nameof(string.EndsWith),
                        new[] { typeof(string) })!,
                    constant),

                _ => throw new ArgumentException(
                    "Неразрешен текстов оператор.")
            };
        }

        private static Expression BuildComparison(
            Expression property,
            string op,
            Expression constant)
        {
            return op switch
            {
                "equals" => Expression.Equal(property, constant),
                "notequals" => Expression.NotEqual(property, constant),
                "gt" => Expression.GreaterThan(property, constant),
                "lt" => Expression.LessThan(property, constant),
                "gte" => Expression.GreaterThanOrEqual(property, constant),
                "lte" => Expression.LessThanOrEqual(property, constant),

                _ => throw new ArgumentException(
                    "Неразрешен оператор за сравнение.")
            };
        }
    }
}
