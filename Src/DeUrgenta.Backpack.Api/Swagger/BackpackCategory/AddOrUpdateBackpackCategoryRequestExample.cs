using DeUrgenta.Backpack.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Backpack.Api.Swagger.BackpackCategory
{
    public class AddOrUpdateBackpackCategoryRequestExample : IExamplesProvider<BackpackCategoryRequest>
    {
        public BackpackCategoryRequest GetExamples()
        {
            return new() { Name = "Medicamente" };
        }
    }
}