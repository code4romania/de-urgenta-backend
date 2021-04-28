using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.QueryHandlers
{
    public class GetMyGroupsHandler : IRequestHandler<GetMyGroups, Result<IImmutableList<GroupModel>>>
    {
        private readonly IValidateRequest<GetMyGroups> _validator;
        private readonly DeUrgentaContext _context;

        public GetMyGroupsHandler(IValidateRequest<GetMyGroups> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<GroupModel>>> Handle(GetMyGroups request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);

            if (!isValid)
            {
                return Result.Failure<IImmutableList<GroupModel>>("Validation failed");
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);

            var groups = await _context.UsersToGroups
                .Include(g => g.Group)
                .Where(userToGroup => userToGroup.UserId == user.Id)
                .Select(x => x.Group)
                .Select(g => new GroupModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    IsAdmin = g.AdminId == user.Id
                })
                .ToListAsync(cancellationToken);

            return groups.ToImmutableList();
        }
    }
}