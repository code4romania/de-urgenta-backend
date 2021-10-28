using System.Collections.Generic;
using DeUrgenta.Common.Models.Dtos;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Common.Swagger
{
    public class TooManyRequestsResponseExample : IExamplesProvider<ActionResponse>
    {
        public ActionResponse GetExamples() =>
            new()
            {
                Success = false,
                Errors = new List<string> { "Too many requests" }
            };
    }
}
