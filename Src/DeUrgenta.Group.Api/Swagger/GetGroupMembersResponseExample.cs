using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DeUrgenta.Group.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Group.Api.Swagger
{
    public class GetGroupMembersResponseExample:IExamplesProvider<ImmutableList<GroupMemberModel>> 
    {
        public ImmutableList<GroupMemberModel> GetExamples()
        {
            return new List<GroupMemberModel>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Freddie",
                    LastName = "Mercury"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Doru",
                    LastName = "Puscas"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Bruno",
                    LastName = "Mars"
                },
            }.ToImmutableList();
        }
    }
}