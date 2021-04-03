using System;
using DeUrgenta.Backpack.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Backpack.Api.Swagger.Backpack
{
    public class AddOrUpdateBackpackResponseExample : IExamplesProvider<BackpackModel>
    {
        public BackpackModel GetExamples()
        {
            return new() { Id = Guid.NewGuid(), Name = "Ruxacul meu de urgenta" };
        }
    }
}