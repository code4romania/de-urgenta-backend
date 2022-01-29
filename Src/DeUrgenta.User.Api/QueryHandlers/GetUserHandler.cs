using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain.Api;
using DeUrgenta.User.Api.Models;
using DeUrgenta.User.Api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.QueryHandlers
{
    public class GetUserHandler : IRequestHandler<GetUser, Result<UserModel, ValidationResult>>
    {
        private readonly IValidateRequest<GetUser> _validator;
        private readonly DeUrgentaContext _context;

        public GetUserHandler(IValidateRequest<GetUser> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<UserModel, ValidationResult>> Handle(GetUser request, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.IsValidAsync(request);
            if (validationResult.IsFailure)
            {
                return validationResult;
            }

            var user = await _context.Users.Where(x => x.Sub == request.UserSub).FirstAsync(cancellationToken);

            return new UserModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
    }
}