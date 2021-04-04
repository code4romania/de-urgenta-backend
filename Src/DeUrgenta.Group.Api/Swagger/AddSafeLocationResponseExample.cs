using System;
using DeUrgenta.Group.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Group.Api.Swagger
{
    public class AddSafeLocationResponseExample : IExamplesProvider<SafeLocationModel>
    {
        public SafeLocationModel GetExamples()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = "Locatia noastra safe",
                Latitude = 99,
                Longitude = 66
            };
        }
    }
}