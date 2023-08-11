using FE.IntegrationTests.Repro.API.Modules.Categories.Models;

namespace FE.IntegrationTests.Repro.API.Modules.Categories.Features.CreateCategory;

public class CreateCategoryEndpoint : ApiEndpoint<CreateCategoryRequest, CategoryDto>
{
    public override void Configure()
    {
        Post("categories");
        Version(1);
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateCategoryRequest req, CancellationToken ct)
    {
        var category = new Category(req.Names.FirstOrDefault()?.Value, req.Icon, req.Order);

        Context.Categories.Add(category);
        await Context.SaveChangesAsync(ct);

        await SendOkAsync(ct);
    }
}
