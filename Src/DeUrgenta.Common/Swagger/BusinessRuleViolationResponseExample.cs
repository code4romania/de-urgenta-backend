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
                Detail = "A business rule was violated. Here you will find a meaningful message of what happened.",
                Status = 400
            };
        }
    }
}
