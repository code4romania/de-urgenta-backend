using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using DeUrgenta.Domain.I18n;
using DeUrgenta.Domain.I18n.Entities;
using DeUrgenta.I18n.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.I18n.Service.Providers
{
    public class ContentProvider : IAmContentProvider
    {

        private readonly I18nDbContext _context;
        private readonly IAmLanguageProvider _languageProvider;

        public ContentProvider(I18nDbContext context, IAmLanguageProvider languageProvider)
        {
            _context = context;
            _languageProvider = languageProvider;
        }

        public async Task<ImmutableList<string>> GetAvailableContentKeys(string culture)
        {
            var language = await _languageProvider.GetLanguageByCulture(culture);

            var resources = await _context.StringResources.Where(x => x.LanguageId == language.Id)
                .Select(r => r.Key)
                .ToListAsync();

            return resources.ToImmutableList();
        }

        public async Task<StringResourceModel> AddOrUpdateContentValue(string culture, string resourceKey, string resourceValue)
        {
            var language = await _languageProvider.GetLanguageByCulture(culture);

            var langRes = await _context.StringResources
                .FirstOrDefaultAsync(str => str.Language.Culture == culture && str.Key == resourceKey);

            if (langRes != null)
            {
                langRes.Value = resourceValue;
                _context.Entry(langRes).State = EntityState.Modified;
            }
            else
            {
                langRes = new StringResource
                {
                    Key = resourceKey,
                    Value = resourceValue,
                    LanguageId = language.Id

                };
            }

            await _context.SaveChangesAsync();

            return new StringResourceModel { Id = langRes.Id, Key = langRes.Key, Value = langRes.Value };
        }
    }
}