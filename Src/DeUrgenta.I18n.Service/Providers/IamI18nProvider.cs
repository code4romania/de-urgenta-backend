using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using DeUrgenta.I18n.Service.Models;

namespace DeUrgenta.I18n.Service.Providers
{
    public interface IamI18nProvider
    {
        Task<IImmutableList<LanguageModel>> GetLanguages();
        Task<LanguageModel> GetLanguageByCulture(string culture);
        Task<StringResourceModel> GetStringResource(string resourceKey, Guid languageId);
        Task<string> Localize(string resourceKey, params object[] args);
    }
}