
using ST2_2026_Team_4_GameLibrary.Models;

namespace ST2_2026_Team_4_GameLibrary.Services;

public static class GameQueryValidator
{
    private static readonly HashSet<string> AllowedFields =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "Id",
            "Title",
            "Genre",
            "Developer",
            "ReleaseDate",
            "Description"
        };

    private static readonly HashSet<string> AllowedOperators =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "contains",
            "equals",
            "startsWith",
            "endsWith",
            "notEquals",
            "gt",
            "lt",
            "gte",
            "lte"
        };

    public static bool TryValidate(
        JSONQuery? query,
        out string errorMessage)
    {
        errorMessage = string.Empty;

        if (query is null)
        {
            errorMessage = "Липсва заявка за търсене.";
            return false;
        }

        if (query.LogicalOperator is null ||
            !(query.LogicalOperator.Equals("AND", StringComparison.OrdinalIgnoreCase) ||
              query.LogicalOperator.Equals("OR", StringComparison.OrdinalIgnoreCase)))
        {
            errorMessage = "Невалиден логически оператор.";
            return false;
        }

        if (query.Conditions is null || query.Conditions.Count > 20)
        {
            errorMessage = "Невалиден списък с условия.";
            return false;
        }

        foreach (var condition in query.Conditions)
        {
            if (condition is null ||
                string.IsNullOrWhiteSpace(condition.Field) ||
                !AllowedFields.Contains(condition.Field))
            {
                errorMessage = "Заявката съдържа неразрешено поле.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(condition.Operator) ||
                !AllowedOperators.Contains(condition.Operator))
            {
                errorMessage = "Заявката съдържа неразрешен оператор.";
                return false;
            }

            if (condition.Value is null || condition.Value.Length > 2000)
            {
                errorMessage = "Невалидна стойност в условието.";
                return false;
            }

            var isComparisonOperator =
                condition.Operator.Equals("gt", StringComparison.OrdinalIgnoreCase) ||
                condition.Operator.Equals("lt", StringComparison.OrdinalIgnoreCase) ||
                condition.Operator.Equals("gte", StringComparison.OrdinalIgnoreCase) ||
                condition.Operator.Equals("lte", StringComparison.OrdinalIgnoreCase);

            if (isComparisonOperator &&
                !condition.Field.Equals("Id", StringComparison.OrdinalIgnoreCase) &&
                !condition.Field.Equals("ReleaseDate", StringComparison.OrdinalIgnoreCase))
            {
                errorMessage =
                    "Операторите за сравнение са разрешени само за Id и ReleaseDate.";
                return false;
            }

            if (condition.Field.Equals("Id", StringComparison.OrdinalIgnoreCase) &&
                !int.TryParse(condition.Value, out _))
            {
                errorMessage = "За Id трябва да се подаде цяло число.";
                return false;
            }

            if (condition.Field.Equals("ReleaseDate", StringComparison.OrdinalIgnoreCase) &&
                !DateTime.TryParse(
                    condition.Value,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out _))
            {
                errorMessage = "За ReleaseDate трябва да се подаде валидна дата.";
                return false;
            }
        }

        if (string.IsNullOrWhiteSpace(query.OrderBy) ||
            !AllowedFields.Contains(query.OrderBy))
        {
            errorMessage = "Невалидно поле за сортиране.";
            return false;
        }

        if (query.SortDirection is not null &&
            !query.SortDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase) &&
            !query.SortDirection.Equals("DESC", StringComparison.OrdinalIgnoreCase))
        {
            errorMessage = "Невалидна посока на сортиране.";
            return false;
        }

        if (query.Limit is < 1 or > 100)
        {
            errorMessage = "Лимитът трябва да е между 1 и 100.";
            return false;
        }

        return true;
    }
}
