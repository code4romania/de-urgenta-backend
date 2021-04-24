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
    public class GetAdministeredGroupsHandler : IRequestHandler<GetAdministeredGroups, Result<IImmutableList<GroupModel>>>
    {
        private readonly IValidateRequest<GetAdministeredGroups> _validator;
        private readonly DeUrgentaContext _context;

        public GetAdministeredGroupsHandler(IValidateRequest<GetAdministeredGroups> validator,
            DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<IImmutableList<GroupModel>>> Handle(GetAdministeredGroups request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<IImmutableList<GroupModel>>("Validation failed");
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);

            var groups = await _context
                .Groups
                .Where(g => g.AdminId == user.Id)
                .Select(g => new GroupModel { Id = g.Id, Name = g.Name, IsAdmin = g.AdminId == user.Id })
                .ToListAsync(cancellationToken);

            return groups.ToImmutableList();
        }
    }
}