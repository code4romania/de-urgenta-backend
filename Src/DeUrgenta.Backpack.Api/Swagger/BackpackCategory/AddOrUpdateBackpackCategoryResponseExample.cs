using System;
using DeUrgenta.Backpack.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Backpack.Api.Swagger.BackpackCategory
{
    public class AddOrUpdateBackpackCategoryResponseExample : IExamplesProvider<BackpackCategoryModel>
    {
        public BackpackCategoryModel GetExamples()
        {
            return new() { Id = Guid.NewGuid(), Name = "Medicamente" };
        }
    }
}