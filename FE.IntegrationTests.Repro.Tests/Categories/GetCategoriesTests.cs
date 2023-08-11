using FastEndpoints;
using FE.IntegrationTests.Repro.API.Core;
using FE.IntegrationTests.Repro.API.Modules.Categories.Features.GetCategories;
using FE.IntegrationTests.Repro.API.Modules.Categories.Models;

namespace FE.IntegrationTests.Repro.Tests.Categories;
public class GetCategoriesTests : BaseApiTest
{
    public GetCategoriesTests(WebFactoryFixture fixture)
        : base(fixture)
    { }

    [Fact]
    public async Task GetCategories_Success()
    {
        using var context = CreateDbContext();
        var client = GetAnnonymousHttpClient();
        var request = new EmptyRequest();

        var (response, result) = await client.GETAsync<GetCategoriesEndpoint, EmptyRequest, Envelope<IReadOnlyList<CategoryDto>>>(request);

        response.IsSuccessStatusCode.Should().BeTrue();        
        result.Result.Should().NotBeEmpty();
        result.Result.Count.Should().Be(context.Categories.Count());
    }
}
