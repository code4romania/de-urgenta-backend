using System.Collections.Immutable;
using System.Threading.Tasks;
using DeUrgenta.I18n.Service.Models;

namespace DeUrgenta.I18n.Service.Providers
{
    public interface IAmLanguageProvider
    {
        Task<LanguageModel> GetLanguageByCulture(string culture);
        Task<IImmutableList<LanguageModel>> GetLanguages();
    }
}