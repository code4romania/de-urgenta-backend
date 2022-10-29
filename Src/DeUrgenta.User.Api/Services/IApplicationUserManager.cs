using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.User.Api.Models.DTOs.Requests;

namespace DeUrgenta.User.Api.Services
{
    public interface IApplicationUserManager
    {
        Task CreateApplicationUserAsync(UserRegistrationDto user, string userSub, CancellationToken cancellationToken);
    }
}