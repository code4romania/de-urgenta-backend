using System.Collections.Generic;
using System.Collections.Immutable;
using DeUrgenta.I18n.Service.Domain;
using DeUrgenta.I18n.Service.Domain.Entities;

namespace DeUrgenta.I18n.Service
{
    public interface ILanguageService
    {
        IImmutableList<Language> GetLanguages();
        Language GetLanguageByCulture(string culture);
    }

    public class LanguageService : ILanguageService
    {
        private readonly I18nDbContext _context;

        public LanguageService(I18nDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Language> GetLanguages()
        {
            return _context.Languages.ToList();
        }

        public Language GetLanguageByCulture(string culture)
        {
            return _context.Languages.FirstOrDefault(x =>
                x.Culture.Trim().ToLower() == culture.Trim().ToLower());
        }
    }

    public interface ILocalizationService
    {
        StringResource GetStringResource(string resourceKey, int languageId);
    }

    public class LocalizationService : ILocalizationService
    {
        private readonly MyAppDbContext _context;

        public LocalizationService(MyAppDbContext context)
        {
            _context = context;
        }

        public StringResource GetStringResource(string resourceKey, int languageId)
        {
            return _context.StringResources.FirstOrDefault(x =>
                x.Name.Trim().ToLower() == resourceKey.Trim().ToLower()
                && x.LanguageId == languageId);
        }
    }
}