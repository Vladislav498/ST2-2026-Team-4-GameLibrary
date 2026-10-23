
namespace ST2_2026_Team_4_GameLibrary.Models;

public sealed class GeminiQueryResult
{
    public bool Success { get; private set; }

    public string? Query { get; private set; }

    public string? RawJson { get; private set; }

    public string? ErrorMessage { get; private set; }

    private GeminiQueryResult()
    {
    }

    public static GeminiQueryResult Ok(string query, string rawJson)
    {
        return new GeminiQueryResult
        {
            Success = true,
            Query = query,
            RawJson = rawJson
        };
    }

    public static GeminiQueryResult Fail(string errorMessage)
    {
        return new GeminiQueryResult
        {
            Success = false,
            ErrorMessage = errorMessage
        };
    }
}
