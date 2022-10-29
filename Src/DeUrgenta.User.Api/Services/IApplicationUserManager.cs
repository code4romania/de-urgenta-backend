using System.Threading.Tasks;
using DeUrgenta.User.Api.Models.DTOs.Requests;

namespace DeUrgenta.User.Api.Services
{
    public interface IApplicationUserManager //TODO add ct support to methods
    {
        Task CreateApplicationUserAsync(UserRegistrationDto user, string userSub);
    }
}