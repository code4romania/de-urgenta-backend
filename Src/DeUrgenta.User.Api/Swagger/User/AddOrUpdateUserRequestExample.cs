using DeUrgenta.User.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.User.Api.Swagger.User
{
    public class AddOrUpdateUserRequestExample : IExamplesProvider<UserRequest>
    {
        public UserRequest GetExamples()
        {
            return new() { FirstName = "Joe", LastName = "Pesci" };
        }
    }
}