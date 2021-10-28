using System.Collections.Generic;
using DeUrgenta.Common.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Common.Swagger
{
    public class ConflictErrorResponseExample : IExamplesProvider<ActionResponse>
    {
        public ActionResponse GetExamples() =>
            new()
            {
                Success = false,
                Errors = new List<string> { "An entity is already defined from the provided identifier"}
            };
    }
}
