namespace FE.IntegrationTests.Repro.API.Modules.Categories.Features.CreateCategory;

public record CreateCategoryRequest(List<TranslationDto> Names, int Order, string Icon);
