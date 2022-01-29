using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Common.Swagger
{
    public class BusinessRuleViolationResponseExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new()
            {
                Type =  "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title= "Bad Request",
                Status =  400
            };
        }
    }
}
