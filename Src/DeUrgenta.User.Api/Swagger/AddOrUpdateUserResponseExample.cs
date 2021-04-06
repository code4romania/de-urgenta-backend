using DeUrgenta.User.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.User.Api.Swagger
{
    public class AddOrUpdateUserResponseExample : IExamplesProvider<UserModel>
    {
        public UserModel GetExamples()
        {
            return new() {FirstName = "Code4", LastName = "Romania"};
        }
    }
}