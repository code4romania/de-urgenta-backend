using DeUrgenta.User.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.User.Api.Swagger
{
    public class AddOrUpdateUserSafeLocationRequestExample : IExamplesProvider<UserSafeLocationRequest>
    {
        public UserSafeLocationRequest GetExamples()
        {
            return new()
            {
                Name = "Locatia noastra safe",
                Longitude = 99,
                Latitude = 66
            };
        }
    }
}