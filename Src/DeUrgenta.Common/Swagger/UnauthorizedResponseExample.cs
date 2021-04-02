using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Common.Swagger
{
    public class UnauthorizedResponseExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new()
            {
                Detail = "A business rule was violated. Here you will find a meaningful message of what happened.",
                Status = 401,
                Title = "Unauthorized",
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1",
            };
        }
    }
}