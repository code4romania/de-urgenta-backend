using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using DeUrgenta.User.Api.Models;
using DeUrgenta.User.Api.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.User.Api.QueryHandlers
{
    public class GetUserHandler : IRequestHandler<GetUser, Result<UserModel>>
    {
        private readonly IValidateRequest<GetUser> _validator;
        private readonly DeUrgentaContext _context;

        public GetUserHandler(IValidateRequest<GetUser> validator, DeUrgentaContext context)
        {
            _validator = validator;
            _context = context;
        }

        public async Task<Result<UserModel>> Handle(GetUser request, CancellationToken cancellationToken)
        {
            var isValid = await _validator.IsValidAsync(request);
            if (!isValid)
            {
                return Result.Failure<UserModel>("Validation failed");
            }

            var user = await _context.Users.Where(x => x.Sub == request.UserSub).FirstAsync(cancellationToken);

            return new UserModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
    }
}