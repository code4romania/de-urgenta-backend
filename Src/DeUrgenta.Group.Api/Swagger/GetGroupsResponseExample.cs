using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DeUrgenta.Group.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Group.Api.Swagger
{
    public class GetGroupsResponseExample : IExamplesProvider<IImmutableList<GroupModel>>
    {
        public IImmutableList<GroupModel> GetExamples()
        {
            return new List<GroupModel>
            {
                new (){Name = "Grup personal", Id = Guid.NewGuid()},
                new (){Name = "Grupul familiei", Id = Guid.NewGuid()},
                new (){Name = "Colegii", Id = Guid.NewGuid()}
            }.ToImmutableList();
        }
    }
}