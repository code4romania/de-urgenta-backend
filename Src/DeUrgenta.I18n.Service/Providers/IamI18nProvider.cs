using System;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.I18n.Service.Models;

namespace DeUrgenta.I18n.Service.Providers
{
    public interface IamI18nProvider
    {
        Task<StringResourceModel> GetStringResource(string resourceKey, Guid languageId, CancellationToken cancellationToken);
        Task<string> Localize(string resourceKey, CancellationToken cancellationToken, params object[] args);
        Task<string> Localize(LocalizableString resource, CancellationToken cancellationToken);
    }
}