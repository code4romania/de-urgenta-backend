using DeUrgenta.Backpack.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Backpack.Api.Swagger.Backpack
{
    public class AddOrUpdateBackpackRequestExample : IExamplesProvider<BackpackModelRequest>
    {
        public BackpackModelRequest GetExamples()
        {
            return new() { Name = "Ruxac nou" };
        }
    }
}