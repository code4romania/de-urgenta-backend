using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using DeUrgenta.Group.Api.Models;
using MediatR;

namespace DeUrgenta.Group.Api.Commands
{
    public class AddGroup : IRequest<Result<GroupModel, ValidationResult>>
    {
        public string UserSub { get; }
        public GroupRequest Group { get; }

        public AddGroup(string userSub, GroupRequest group)
        {
            UserSub = userSub;
            Group = group;
        }
    }
}