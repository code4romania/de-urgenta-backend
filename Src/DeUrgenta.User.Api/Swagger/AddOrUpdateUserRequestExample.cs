using DeUrgenta.User.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.User.Api.Swagger
{
    public class AddOrUpdateUserRequestExample : IExamplesProvider<UserRequest>
    {
        public UserRequest GetExamples()
        {
            return new() { FirstName = "Code4", LastName = "Romania" };
        }
    }
}