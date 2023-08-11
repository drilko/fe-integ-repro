namespace FE.IntegrationTests.Repro.API.Seed
{
    public class Seeder
    {
        private readonly MyDbContext _context;

        public Seeder(MyDbContext context)
        {
            _context = context;
        }

        public async Task ExecuteAsync()
        {
            var categories = new List<Category>
            {
                new Category("category 1", "category1.svg", 1),
                new Category("category 2", "category2.svg", 2),
                new Category("category 2", "category2.svg", 3)
            };
            _context.Categories.AddRange(categories);

            var languages = new List<Language>
            {
                new Language("English", "en"),
                new Language("Croatian", "hr"),
            };
            _context.Languages.AddRange(languages);

            await _context.SaveChangesAsync();
        }
    }
}
