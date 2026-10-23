using ST2_2026_Team_4_GameLibrary.Models;
namespace ST2_2026_Team_4_GameLibrary.Services;

public interface IGeminiQueryService
{
    Task<GeminiQueryResult> GenerateJsonQueryAsync(
        string naturalLanguagePrompt,
        CancellationToken cancellationToken = default);
}
