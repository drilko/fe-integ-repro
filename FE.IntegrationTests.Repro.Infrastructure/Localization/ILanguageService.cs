namespace FE.IntegrationTests.Repro.Infrastructure.Localization;

public interface ILanguageService
{
    /// <summary>
    /// Gets a list of languages.
    /// </summary>
    /// <param name="name">Name to filter on.</param>
    /// <param name="isPublished">Filter for IsPublished.</param>
    /// <param name="ct">CancellationToken</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the list of languages
    /// </returns>
    Task<IReadOnlyList<Language>> GetLanguagesAsync(CancellationToken ct = default);
}
