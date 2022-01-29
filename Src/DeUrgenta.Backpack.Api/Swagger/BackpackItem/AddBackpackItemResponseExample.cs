using System;
using DeUrgenta.Backpack.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Backpack.Api.Swagger.BackpackItem
{
    public class AddBackpackItemResponseExample : IExamplesProvider<BackpackItemRequest>
    {
        public BackpackItemRequest GetExamples()
        {
            return new()
            {
                Name = "Hering conserva",
                Amount = 5,
                ExpirationDate = DateTime.Today.AddDays(720),
                Version = 3
            };
        }
    }
}