using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Options;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DeUrgenta.Group.Api.CommandHandlers
{
    public class UpdateGroupHandler : IRequestHandler<UpdateGroup, Result<GroupModel>>
    {
        private readonly IValidateRequest<UpdateGroup> _validator;
        private readonly DeUrgentaContext _context;
        private readonly GroupsConfig _groupsConfig;

        public UpdateGroupHandler(IValidateRequest<UpdateGroup> validator, DeUrgentaContext context,
            IOptions<GroupsConfig> groupsConfig)
        {
            _validator = validator;
            _context = context;
            _groupsConfig = groupsConfig.Value;
        }

        public async Task<Result<GroupModel>> Handle(UpdateGroup request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<GroupModel>("Validation failed");
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);
            var group = await _context
                .Groups
                .Include(g => g.Admin)
                .FirstAsync(g => g.AdminId == user.Id && g.Id == request.GroupId, cancellationToken);

            group.Name = request.Group.Name;

            await _context.SaveChangesAsync(cancellationToken);

            return new GroupModel
            {
                Id = group.Id,
                Name = group.Name,
                NumberOfMembers = group.GroupMembers.Count,
                MaxNumberOfMembers = _groupsConfig.UsersLimit,
                IsCurrentUserAdmin = group.AdminId == user.Id
            };
        }
    }
}