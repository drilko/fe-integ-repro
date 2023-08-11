using FE.IntegrationTests.Repro.API.Modules.Categories.Models;
using Microsoft.EntityFrameworkCore;

namespace FE.IntegrationTests.Repro.API.Modules.Categories.Features.GetCategories;

public class GetCategoriesEndpoint : ApiEndpoint<EmptyRequest, IReadOnlyList<CategoryDto>>
{
    public override void Configure()
    {
        Get("categories");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var categories = await Context.Categories.ProjectToType<CategoryDto>().ToListAsync(ct);

        await SendOkAsync(categories, ct);
    }
}
