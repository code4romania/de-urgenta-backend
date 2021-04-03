using DeUrgenta.Group.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Group.Api.Swagger
{
    public class AddOrUpdateSafeLocationRequestExample : IExamplesProvider<SafeLocationRequest>
    {
        public SafeLocationRequest GetExamples()
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