using FE.IntegrationTests.Repro.API.Core;
using FE.IntegrationTests.Repro.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Data.Common;

namespace FE.IntegrationTests.Repro.Tests;
public class WebFactoryFixture : WebApplicationFactory<Envelope>
{
    private static readonly object _lock = new();
    private static DbContextOptions<MyDbContext> _dbContextOptions;
    private static bool _databaseInitialized;
    private readonly ConcurrentDictionary<string, HttpClient> _httpClientDictionary = new();
    public DbContextOptions<MyDbContext> DbContextOptions => _dbContextOptions ?? CreateDbContextOptions();
    public WebFactoryFixture()
        : base()
    {
        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                using var context = new MyDbContext(DbContextOptions, false);
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                SeedDatabase(context);
                _databaseInitialized = true;
            }
        }
    }
    public HttpClient GetHttpClient(string name, string userId = null)
    {
        if (!_httpClientDictionary.TryGetValue(name, out var httpClient))
        {
            httpClient = CreateClient();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                httpClient.DefaultRequestHeaders.Add("X-FakeUserId", userId);
            }
            _httpClientDictionary.TryAdd(name, httpClient);
        }

        return httpClient;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        builder.UseEnvironment("Development");
        builder.ConfigureLogging(logging => logging.ClearProviders());

        builder.ConfigureTestServices(services =>
        {
            var dbContextDescriptor = services.SingleOrDefault(
               d => d.ServiceType ==
                   typeof(DbContextOptions<MyDbContext>));
            services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbConnection));
            services.Remove(dbConnectionDescriptor);

            services.RemoveAll(typeof(MyDbContext));

            services.AddScoped<MyDbContext>(sp =>
            {
                var context = new MyDbContext(DbContextOptions);

                return context;
            });
        });
    }
    private static void SeedDatabase(MyDbContext context)
    {
        var seeder = new API.Seed.Seeder(context);

        seeder.ExecuteAsync().GetAwaiter().GetResult();
    }

    private DbContextOptions<MyDbContext> CreateDbContextOptions()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = System.IO.Path.Join(path, "mydb_tests.db");

        _dbContextOptions = new DbContextOptionsBuilder<MyDbContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;


        return _dbContextOptions;
    }
}
