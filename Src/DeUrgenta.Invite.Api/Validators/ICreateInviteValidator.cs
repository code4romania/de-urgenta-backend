using System.Threading;
using System.Threading.Tasks;
using DeUrgenta.Common.Validation;
using DeUrgenta.Invite.Api.Commands;

namespace DeUrgenta.Invite.Api.Validators
{
    public interface ICreateInviteValidator
    {
        Task<ValidationResult> ValidateAsync(CreateInvite request, CancellationToken cancellationToken);
    }
}