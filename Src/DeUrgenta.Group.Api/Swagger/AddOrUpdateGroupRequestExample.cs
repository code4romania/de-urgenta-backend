using DeUrgenta.Group.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Group.Api.Swagger
{
    public class AddOrUpdateGroupRequestExample : IExamplesProvider<GroupModelRequest>
    {
        public GroupModelRequest GetExamples()
        {
            return new() { Name = "Ruxac nou" };
        }
    }
}