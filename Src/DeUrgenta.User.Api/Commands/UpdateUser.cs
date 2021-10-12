using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.User.Api.Models;
using MediatR;

namespace DeUrgenta.User.Api.Commands
{
    public class UpdateUser : IRequest<Result<Unit, ValidationResult>>
    {
        public string UserSub { get; }
        public UserRequest UserDetails { get; }

        public UpdateUser(string userSub, UserRequest userDetails)
        {
            UserSub = userSub;
            UserDetails = userDetails;
        }
    }
}