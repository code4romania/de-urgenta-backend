using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Options;
using DeUrgenta.Group.Api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Group.Api.QueryHandlers
{
    public class
        GetAdministeredGroupsHandler : IRequestHandler<GetAdministeredGroups, Result<IImmutableList<GroupModel>, ValidationResult>>
    {
        private readonly IValidateRequest<GetAdministeredGroups> _validator;
        private readonly DeUrgentaContext _context;
        private readonly GroupsConfig _groupsConfig;

        public GetAdministeredGroupsHandler(IValidateRequest<GetAdministeredGroups> validator,
            DeUrgentaContext context, IOptions<GroupsConfig> groupsConfig)
        {
            _validator = validator;
            _context = context;
            _groupsConfig = groupsConfig.Value;
        }

        public async Task<Result<IImmutableList<GroupModel>, ValidationResult>> Handle(GetAdministeredGroups request,
            CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);

            var groups = await _context
                .Groups
                .Where(g => g.AdminId == user.Id)
                .Include(g => g.Admin)
                .Select(g => new GroupModel
                {
                    Id = g.Id,
                    Name = g.Name,
                    NumberOfMembers = g.GroupMembers.Count,
                    MaxNumberOfMembers = _groupsConfig.UsersLimit,
                    IsCurrentUserAdmin = g.AdminId == user.Id

                })
                .ToListAsync(cancellationToken);

            return groups.ToImmutableList();
        }
    }
}