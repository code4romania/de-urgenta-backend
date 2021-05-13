using CSharpFunctionalExtensions;
using DeUrgenta.Group.Api.Models;
using MediatR;

namespace DeUrgenta.Group.Api.Commands
{
    public class AddGroup : IRequest<Result<GroupModel>>
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