
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using ST2_2026_Team_4_GameLibrary.Configuration;
using ST2_2026_Team_4_GameLibrary.Models;

namespace ST2_2026_Team_4_GameLibrary.Services;

public sealed class GeminiQueryService : IGeminiQueryService
{
    private readonly HttpClient _httpClient;
    private readonly ExternalApisOptions _options;
    private readonly ILogger<GeminiQueryService> _logger;

    public GeminiQueryService(
        HttpClient httpClient,
        IOptions<ExternalApisOptions> options,
        ILogger<GeminiQueryService> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    private const string SystemPrompt = """
        Ти си асистент за търсене в каталог с видеоигри.
        Преобразувай въпроса на потребителя в единствено валиден JSON обект.
        Не генерирай SQL код и не добавяй текст извън JSON обекта.

        Разрешени полета:
        Id, Title, Genre, Developer, ReleaseDate, Description.

        Разрешени оператори:
        contains, equals, startsWith, endsWith, notEquals, gt, lt, gte, lte.

        logicalOperator трябва да е AND или OR.
        orderBy трябва да е едно от разрешените полета.
        sortDirection трябва да е ASC или DESC.
        limit трябва да е цяло число между 1 и 100 или null.

        Използвай следната JSON структура:
        {
          "logicalOperator": "AND",
          "conditions": [
            {
              "field": "Genre",
              "operator": "equals",
              "value": "RPG"
            }
          ],
          "orderBy": "Title",
          "sortDirection": "ASC",
          "limit": 20,
          "explanation": "Търсене на RPG игри"
        }

        Ако потребителят не е посочил условие, върни празен масив conditions.
        За дати използвай формат yyyy-MM-dd.
        """;

    public async Task<GeminiQueryResult> GenerateJsonQueryAsync(
        string naturalLanguagePrompt,
        CancellationToken cancellationToken = default)
    {

if (string.IsNullOrWhiteSpace(naturalLanguagePrompt))
{
    return GeminiQueryResult.Fail("Моля, въведете въпрос за търсене.");
}

if (string.IsNullOrWhiteSpace(_options.GeminiApiKey))
{
    return GeminiQueryResult.Fail(
        "Gemini API ключът не е конфигуриран.");
}

try
{
    var model = Uri.EscapeDataString(_options.GeminiModel);
    var apiKey = Uri.EscapeDataString(_options.GeminiApiKey);

    var url =
        $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent?key={apiKey}";

    var requestBody = new
    {
        systemInstruction = new
        {
            parts = new[]
            {
                new { text = SystemPrompt }
            }
        },
        contents = new[]
        {
            new
            {
                role = "user",
                parts = new[]
                {
                    new { text = naturalLanguagePrompt }
                }
            }
        },
        generationConfig = new
        {
            temperature = 0.1,
            responseMimeType = "application/json"
        }
    };

    var json = JsonSerializer.Serialize(requestBody);

    using var content = new StringContent(
        json, Encoding.UTF8, "application/json");

    using var response = await _httpClient.PostAsync(
        url, content, cancellationToken);

    if (!response.IsSuccessStatusCode)
    {
        _logger.LogWarning(
            "Gemini API върна HTTP статус {StatusCode}.",
            (int)response.StatusCode);

        return GeminiQueryResult.Fail(
            response.StatusCode == HttpStatusCode.TooManyRequests
                ? "Достигнат е лимитът на заявките към Gemini. Опитайте по-късно."
                : "Gemini не успя да обработи заявката.");
    }

    using var responseStream =
        await response.Content.ReadAsStreamAsync(cancellationToken);

    using var document =
        await JsonDocument.ParseAsync(
            responseStream, cancellationToken: cancellationToken);

    if (!document.RootElement.TryGetProperty(
            "candidates", out var candidates) ||
        candidates.GetArrayLength() == 0)
    {
        return GeminiQueryResult.Fail(
            "Gemini не върна резултат.");
    }

    var parts = candidates[0]
        .GetProperty("content")
        .GetProperty("parts");

    var responseText = string.Concat(
        parts.EnumerateArray()
            .Where(p => p.TryGetProperty("text", out _))
            .Select(p => p.GetProperty("text").GetString()));

    if (string.IsNullOrWhiteSpace(responseText))
    {
        return GeminiQueryResult.Fail(
            "Gemini върна празен отговор.");
    }

    using var queryDocument = JsonDocument.Parse(responseText);

    if (queryDocument.RootElement.ValueKind != JsonValueKind.Object)
    {
        return GeminiQueryResult.Fail(
            "Gemini не върна валиден JSON обект.");
    }

    return GeminiQueryResult.Ok(
        responseText, responseText);
}
catch (OperationCanceledException)
    when (cancellationToken.IsCancellationRequested)
{
    return GeminiQueryResult.Fail("Заявката беше отменена.");
}
catch (OperationCanceledException)
{
    return GeminiQueryResult.Fail(
        "Заявката към Gemini отне твърде много време.");
}
catch (HttpRequestException ex)
{
    _logger.LogError(ex, "Грешка при връзка с Gemini API.");

    return GeminiQueryResult.Fail(
        "Неуспешна връзка с Gemini.");
}
catch (JsonException ex)
{
    _logger.LogWarning(ex, "Gemini върна невалиден JSON.");

    return GeminiQueryResult.Fail(
        "Отговорът от Gemini не е валиден JSON.");
}

    }
}
