using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.User.Api.Commands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.CommandHandlers
{
    public class UpdateUserHandler : IRequestHandler<UpdateUser, Result>
    {
        private readonly IValidateRequest<UpdateUser> _validator;
        private readonly DeUrgentaContext _context;

        public UpdateUserHandler(IValidateRequest<UpdateUser> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result> Handle(UpdateUser request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure("Validation failed");
            }

            var user = await _context.Users.FirstAsync(u => u.Sub == request.UserSub, cancellationToken);

            user.FirstName = request.UserDetails.FirstName;
            user.LastName = request.UserDetails.LastName;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}