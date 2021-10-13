using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.User.Api.Models;
using MediatR;

namespace DeUrgenta.User.Api.Queries
{
    public class GetUser : IRequest<Result<UserModel, ValidationResult>>
    {
        public string UserSub { get; }

        public GetUser(string userSub)
        {
            UserSub = userSub;
        }
    }
}