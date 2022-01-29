using System;
using System.Threading.Tasks;
using DeUrgenta.I18n.Service.Models;

namespace DeUrgenta.I18n.Service.Providers
{
    public interface IamI18nProvider
    {
        Task<StringResourceModel> GetStringResource(string resourceKey, Guid languageId);

        Task<string> Localize(string resourceKey, params object[] args);
        Task<string> Localize(LocalizableString resource);
    }
}