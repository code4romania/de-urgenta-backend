using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Certifications.Api.Swagger
{
    public class ApplicationErrorResponseExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new ProblemDetails()
            {
                Detail = "There was an unhandled exception in the backend",
                Status = 500,
                Type = "https://httpstatuses.com/500",
                Title = "Internal Server Error",

            };
        }
    }
}
