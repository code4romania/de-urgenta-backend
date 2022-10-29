using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Invite.Api.Commands;

namespace DeUrgenta.Invite.Api.Validators
{
    public interface ICreateInviteValidator //TODO add ct support to methods
    {
        Task<ValidationResult> ValidateAsync(CreateInvite request);
    }
}