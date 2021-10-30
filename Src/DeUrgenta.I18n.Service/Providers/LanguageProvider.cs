using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Domain.I18n;
using DeUrgenta.I18n.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.I18n.Service.Providers
{
    public class LanguageProvider : IAmLanguageProvider
    {
        private readonly I18nDbContext _context;

        public LanguageProvider(I18nDbContext context)
        {
            _context = context;
        }

        public async Task<IImmutableList<LanguageModel>> GetLanguages()
        {
            var languages = await _context.Languages
                .Select(x => new LanguageModel { Id = x.Id, Culture = x.Culture, Name = x.Name })
                .ToListAsync();

            return languages
                .ToImmutableList();
        }

        public async Task<LanguageModel> GetLanguageByCulture(string culture)
        {
            var language = await _context
                .Languages
                .FirstOrDefaultAsync(x => x.Culture.Trim().ToLower() == culture.Trim().ToLower());

            return language == null ? null : new LanguageModel { Id = language.Id, Culture = language.Culture, Name = language.Name };
        }
    }
}