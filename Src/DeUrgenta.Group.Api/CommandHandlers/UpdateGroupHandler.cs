using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Validators;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.CommandHandlers
{
    public class UpdateGroupHandler : IRequestHandler<UpdateGroup, Result<GroupModel>>
    {
        private readonly IValidateRequest<UpdateGroup> _validator;
        private readonly DeUrgentaContext _context;

        public UpdateGroupHandler(IValidateRequest<UpdateGroup> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<GroupModel>> Handle(UpdateGroup request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<GroupModel>("Validation failed");
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);
            var group = await _context.Groups.FirstAsync(g => g.AdminId == user.Id && g.Id == request.GroupId, cancellationToken);

            group.Name = request.Group.Name;

            await _context.SaveChangesAsync(cancellationToken);

            return new GroupModel { Id = group.Id, Name = group.Name, IsAdmin = true };
        }
    }
}