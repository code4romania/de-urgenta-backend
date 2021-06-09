using CSharpFunctionalExtensions;
using DeUrgenta.User.Api.Models;
using MediatR;

namespace DeUrgenta.User.Api.Commands
{
    public class UpdateUser : IRequest<Result>
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