using FE.IntegrationTests.Repro.Infrastructure.Persistence;

namespace FE.IntegrationTests.Repro.Tests
{
    public class BaseApiTest : IClassFixture<WebFactoryFixture>
    {
        private readonly WebFactoryFixture _fixture;

        public static Faker Faker { get; } = new Faker();

        public BaseApiTest(WebFactoryFixture fixture)
        {
            _fixture = fixture;
        }

        public MyDbContext CreateDbContext()
        => new(_fixture.DbContextOptions, false);

        public HttpClient GetAnnonymousHttpClient() => _fixture.GetHttpClient("Annonymous");
    }
}
