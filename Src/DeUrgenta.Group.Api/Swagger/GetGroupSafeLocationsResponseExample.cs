using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using DeUrgenta.Group.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Group.Api.Swagger
{
    public class GetGroupSafeLocationsResponseExample : IExamplesProvider<IImmutableList<SafeLocationModel>>
    {
        public IImmutableList<SafeLocationModel> GetExamples()
        {
            return new List<SafeLocationModel>
            {
                new()
                {
                    Name = "Locatia noastra safe1",
                    Latitude = 99,
                    Longitude = 55,
                    Id = Guid.NewGuid()
                },
                new()
                {
                    Name = "Locatia noastra safe2",
                    Latitude = 88,
                    Longitude = 44,
                    Id = Guid.NewGuid()
                },
                new()
                {
                    Name = "Beer zone",
                    Latitude = 77,
                    Longitude = 66,
                    Id = Guid.NewGuid()
                }
            }.ToImmutableList();
        }
    }
}