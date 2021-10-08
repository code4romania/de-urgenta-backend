using System.Threading.Tasks;
using DeUrgenta.Invite.Api.Commands;

namespace DeUrgenta.Invite.Api.Validators
{
    public interface ICreateInviteValidator
    {
        Task<bool> ValidateAsync(CreateInvite request);
    }
}