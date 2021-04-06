using System;
using DeUrgenta.User.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.User.Api.Swagger
{
    public class AddUserSafeLocationResponseExample : IExamplesProvider<UserSafeLocationModel>
    {
        public UserSafeLocationModel GetExamples()
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