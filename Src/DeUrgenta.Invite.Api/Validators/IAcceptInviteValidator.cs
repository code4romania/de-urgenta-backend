using System.Threading.Tasks;
using DeUrgenta.Invite.Api.Commands;

namespace DeUrgenta.Invite.Api.Validators
{
    public interface IAcceptInviteValidator
    {
        Task<bool> ValidateAsync(AcceptInvite request);
    }
}