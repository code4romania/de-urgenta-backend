using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DeUrgenta.Backpack.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Backpack.Api.Swagger.Backpack
{
    public class GetBackpacksResponseExample : IExamplesProvider<IImmutableList<BackpackModel>>
    {
        public IImmutableList<BackpackModel> GetExamples()
        {
            return new List<BackpackModel>
            {
                new (){Name = "Personal", Id = Guid.NewGuid()},
                new (){Name = "Ruxacul familiei", Id = Guid.NewGuid()},
                new (){Name = "Ruxacul prietenilor", Id = Guid.NewGuid()}
            }.ToImmutableList();
        }
    }
}