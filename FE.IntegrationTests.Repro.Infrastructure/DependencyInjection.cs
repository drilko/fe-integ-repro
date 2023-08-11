using FE.IntegrationTests.Repro.Infrastructure.Localization;
using FE.IntegrationTests.Repro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FE.IntegrationTests.Repro.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<MyDbContext>(options =>
            {
                var folder = Environment.SpecialFolder.LocalApplicationData;
                var path = Environment.GetFolderPath(folder);
                var dbPath = System.IO.Path.Join(path, "mydb.db");
                options.UseSqlite($"Data Source={dbPath}");
            });

            services.AddScoped<ILanguageService, LanguageService>();

            return services;
        }
    }
}
