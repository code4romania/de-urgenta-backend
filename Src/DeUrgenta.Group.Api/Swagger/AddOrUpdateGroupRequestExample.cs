using DeUrgenta.Group.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Group.Api.Swagger
{
    public class AddOrUpdateGroupRequestExample : IExamplesProvider<GroupRequest>
    {
        public GroupRequest GetExamples()
        {
            return new() {Name = "Grup nou"};
        }
    }
}