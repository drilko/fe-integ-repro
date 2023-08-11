using FE.IntegrationTests.Repro.Infrastructure.Localization;
using FluentValidation;

namespace FE.IntegrationTests.Repro.API.Modules.Categories.Features.CreateCategory;

public class CreateCategoryValidator : Validator<CreateCategoryRequest>
{
    public CreateCategoryValidator()
    {
        RuleFor(x => x.Order)
            .GreaterThan(0)
            .WithError(Errors.General.ValueIsTooSmall(0));

        RuleFor(x => x.Icon)
            .NotEmpty()
            .WithError(Errors.General.ValueIsRequired());

        RuleFor(x => x.Names)
             .MustAsync(AllLanguagesProvided);             
    }

    private async Task<bool> AllLanguagesProvided(List<TranslationDto> translations, CancellationToken token)
    {
        var languageService = Resolve<ILanguageService>();

        var languages = await languageService.GetLanguagesAsync(ct: token);

        return languages.All(x => translations.Exists(t => t.LanguageId == x.Id));
    }
}
