namespace FE.IntegrationTests.Repro.API.Modules.Categories.Models;

public record CategoryDto : BaseDto<CategoryDto, Category>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public int Order { get; set; }
}
