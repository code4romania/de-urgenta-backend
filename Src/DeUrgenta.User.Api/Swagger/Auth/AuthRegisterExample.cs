using DeUrgenta.User.Api.Models;
using DeUrgenta.User.Api.Models.DTOs.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.User.Api.Swagger.User
{
    public class AuthRegisterExample : IExamplesProvider<UserRegistrationDto>
    {
        public UserRegistrationDto GetExamples()
        {
            return new()
            {
                Email = "user@domain.test",
                Password = "S0meL3ngthyPass!",
                FirstName = "Foo",
                LastName = "Bar"
            };
        }
    }
}