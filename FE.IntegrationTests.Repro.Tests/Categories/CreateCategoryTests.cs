using FastEndpoints;
using FE.IntegrationTests.Repro.API.Core;
using FE.IntegrationTests.Repro.API.Modules.Categories.Features.CreateCategory;
using Microsoft.EntityFrameworkCore;

using Xunit.Abstractions;

namespace FE.IntegrationTests.Repro.Tests.Categories;
public class CreateCategoryTests : BaseApiTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    public static IEnumerable<object[]> InvalidCategoryData => new List<object[]>
        {
            new object[] { false, 10, Faker.Image.LoremPixelUrl() },
            new object[] { true, -1, Faker.Image.LoremPixelUrl() },
            new object[] { true, 0, Faker.Image.LoremPixelUrl() },
            new object[] { true, 0, string.Empty },
            new object[] { true, 0, " " },
        };
    public CreateCategoryTests(WebFactoryFixture fixture, ITestOutputHelper testOutputHelper)
        : base(fixture)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Fact]
    public async Task CreateCategory_Success()
    {
        // Arrange
        using var context = CreateDbContext();
        var client = GetAnnonymousHttpClient();

        var translations = context.Languages.Select(x => new TranslationDto { Value = Faker.Random.Word(), LanguageId = x.Id }).ToList();
        var expectedValue = translations.FirstOrDefault()?.Value;

        var request = new CreateCategoryRequest(translations, 10, "categoryx.svg");

        // Act
        var (res, envelope) = await client.POSTAsync<CreateCategoryEndpoint, CreateCategoryRequest, API.Core.EmptyResponse>(request);

        // Assert
        _testOutputHelper.WriteLine(await res.Content.ReadAsStringAsync());
        res.IsSuccessStatusCode.Should().BeTrue();
        (await context.Categories.AnyAsync(x => x.Name == expectedValue)).Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(InvalidCategoryData))]
    public async Task CreateCategory_InvalidData_BadRequest(bool allTranslations, int order, string icon)
    {
        // Arrange
        using var context = CreateDbContext();
        var client = GetAnnonymousHttpClient();

        var languagesCount = context.Languages.Count();
        var translations = context.Languages.Take(allTranslations ? languagesCount : languagesCount - 1).Select(x => new TranslationDto { LanguageId = x.Id, Value = Faker.Random.Word() });
        var request = new CreateCategoryRequest(translations.ToList(), order, icon);


        // Act
        var (res, envelope) = await client.POSTAsync<CreateCategoryEndpoint, CreateCategoryRequest, API.Core.EmptyResponse>(request);

        // Assert
        _testOutputHelper.WriteLine(await res.Content.ReadAsStringAsync());
        res.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
    }
}
