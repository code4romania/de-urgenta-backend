using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.Group.Api.Commands;
using DeUrgenta.Group.Api.Models;
using DeUrgenta.Group.Api.Validators;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Group.Api.CommandHandlers
{
    public class AddGroupHandler : IRequestHandler<AddGroup, Result<GroupModel>>
    {
        private readonly IValidateRequest<AddGroup> _validator;
        private readonly DeUrgentaContext _context;

        public AddGroupHandler(IValidateRequest<AddGroup> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<GroupModel>> Handle(AddGroup request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<GroupModel>("Validation failed");
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);
            var group = new Domain.Entities.Group { Admin = user, Name = request.Group.Name };
            var newGroup = await _context.Groups.AddAsync(group, cancellationToken);

            return new GroupModel { Id = newGroup.Entity.Id, Name = newGroup.Entity.Name, IsAdmin = true };
        }
    }
}