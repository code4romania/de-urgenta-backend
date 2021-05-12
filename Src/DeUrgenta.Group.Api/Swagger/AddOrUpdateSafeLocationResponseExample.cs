using System;
using DeUrgenta.Group.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Group.Api.Swagger
{
    public class AddOrUpdateSafeLocationResponseExample : IExamplesProvider<SafeLocationResponseModel>
    {
        public SafeLocationResponseModel GetExamples()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                GroupId = Guid.NewGuid(),
                Name = "Locatia noastra safe",
                Latitude = 99,
                Longitude = 66
            };
        }
    }
}