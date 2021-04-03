using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DeUrgenta.Backpack.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Backpack.Api.Swagger.Backpack
{
    public class GetBackpackContributorsResponseExample:IExamplesProvider<ImmutableList<BackpackContributorModel>> 
    {
        public ImmutableList<BackpackContributorModel> GetExamples()
        {
            return new List<BackpackContributorModel>
            {
                new()
                {
                    UserId = Guid.NewGuid(),
                    FirstName = "Freddie",
                    LastName = "Mercury"
                },
                new()
                {
                    UserId = Guid.NewGuid(),
                    FirstName = "Doru",
                    LastName = "Puscas"
                },
                new()
                {
                    UserId = Guid.NewGuid(),
                    FirstName = "Bruno",
                    LastName = "Mars"
                },
            }.ToImmutableList();
        }
    }
}