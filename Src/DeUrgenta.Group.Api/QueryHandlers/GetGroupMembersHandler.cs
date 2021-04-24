using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Queries;
using DeUrgenta.Group.Api.Validators;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.QueryHandlers
{
    public class GetGroupMembersHandler : IRequestHandler<GetGroupMembers, Result<IImmutableList<GroupMemberModel>>>
    {
        private readonly IValidateRequest<GetGroupMembers> _validator;
        private readonly DeUrgentaContext _context;

        public GetGroupMembersHandler(IValidateRequest<GetGroupMembers> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<GroupMemberModel>>> Handle(GetGroupMembers request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<IImmutableList<GroupMemberModel>>("Validation failed");
            }

            var groupMembers = await _context
                .UsersToGroups
                .Where(x => x.GroupId == request.GroupId)
                .Include(x => x.User)
                .Select(x => x.User)
                .Select(x => new GroupMemberModel { Id = x.Id, FirstName = x.FirstName, LastName = x.LastName })
                .ToListAsync(cancellationToken);

            return groupMembers.ToImmutableList();
        }
    }
}