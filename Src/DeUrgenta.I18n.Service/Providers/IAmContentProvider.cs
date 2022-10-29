using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.I18n.Service.Models;

namespace DeUrgenta.I18n.Service.Providers
{
    public interface IAmContentProvider
    {
        Task<StringResourceModel> AddOrUpdateContentValue(string culture, string resourceKey, string resourceValue, CancellationToken cancellationToken);

        Task<ImmutableList<string>> GetAvailableContentKeys(string culture, CancellationToken cancellationToken);
    }
}