
namespace ST2_2026_Team_4_GameLibrary.Configuration;

/// <summary>
/// Настройки за външните API услуги.
/// </summary>
public sealed class ExternalApisOptions
{
    public const string SectionName = "ExternalApis";

    public string? GeminiApiKey { get; set; }

    public string GeminiModel { get; set; } = "gemini-3-flash-preview";
}
