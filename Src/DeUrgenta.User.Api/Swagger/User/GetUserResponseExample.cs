using DeUrgenta.User.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.User.Api.Swagger.User
{
    public class GetUserResponseExample : IExamplesProvider<UserModel>
    {
        public UserModel GetExamples()
        {
            return new()
            {
                LastName = "Mercury",
                FirstName = "Freddie"
            };
        }
    }
}