using FE.IntegrationTests.Repro.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FE.IntegrationTests.Repro.Infrastructure.Localization
{
    internal class LanguageService : ILanguageService
    {
        private readonly MyDbContext _context;

        public LanguageService(MyDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<Language>> GetLanguagesAsync(CancellationToken ct = default)
        {
            return await _context.Languages.ToListAsync(ct);
        }
    }
}
