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
                new (){Name = "Personal", Id = Guid.NewGuid()},
                new (){Name = "Ruxacul familiei", Id = Guid.NewGuid()},
                new (){Name = "Ruxacul prietenilor", Id = Guid.NewGuid()}
            }.ToImmutableList();
        }
    }
}