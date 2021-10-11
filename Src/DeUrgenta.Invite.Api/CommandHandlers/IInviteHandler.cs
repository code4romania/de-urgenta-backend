using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Invite.Api.Commands;
using DeUrgenta.Invite.Api.Models;

namespace DeUrgenta.Invite.Api.CommandHandlers
{
    public interface IInviteHandler
    {
        Task<Result<AcceptInviteModel>> HandleAsync(AcceptInvite request, CancellationToken cancellationToken);
    }
}