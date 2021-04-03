using System;
using DeUrgenta.Group.Api.Models;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Group.Api.Swagger
{
    public class AddOrUpdateGroupResponseExample : IExamplesProvider<GroupModel>
    {
        public GroupModel GetExamples()
        {
            return new() { Id = Guid.NewGuid(), Name = "Ruxacul meu de urgenta" };
        }
    }
}