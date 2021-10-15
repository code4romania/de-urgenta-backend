using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Domain.I18n;
using DeUrgenta.I18n.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.I18n.Service.Providers
{
    internal class I18NProvider : IamI18nProvider
    {
        private readonly I18nDbContext _context;

        public I18NProvider(I18nDbContext context)
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

        public async Task<StringResourceModel> GetStringResource(string resourceKey, Guid languageId)
        {
            var resource = await _context
                .StringResources
                .FirstOrDefaultAsync(x => x.Key.Trim().ToLower() == resourceKey.Trim().ToLower()
                    && x.LanguageId == languageId);

            return resource == null
                ? null
                : new StringResourceModel { Id = resource.Id, Value = resource.Value, Key = resource.Key };
        }

        public async Task<string> Localize(string resourceKey, params object[] args)
        {
            var currentCulture = Thread.CurrentThread.CurrentUICulture.Name;

            var language = await GetLanguageByCulture(currentCulture);
            if (language != null)
            {
                var stringResource = await GetStringResource(resourceKey, language.Id);
                if (stringResource == null || string.IsNullOrEmpty(stringResource.Value))
                {
                    return resourceKey;
                }

                return (args == null || args.Length == 0)
                    ? stringResource.Value
                    : string.Format(stringResource.Value, args);
            }

            return resourceKey;
        }
    }
}
