using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.User.Api.Models;
using MediatR;

namespace DeUrgenta.User.Api.Commands
{
    public class AddLocation : IRequest<Result<UserLocationModel, ValidationResult>>
    {
        public string UserSub { get; }
        public UserLocationRequest Location { get; }

        public AddLocation(string userSub, UserLocationRequest location)
        {
            UserSub = userSub;
            Location = location;
        }
    }
}