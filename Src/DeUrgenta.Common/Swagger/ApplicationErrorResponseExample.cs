using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace DeUrgenta.Common.Swagger
{
    /// <summary>
    /// Provides an exaple of application error response
    /// </summary>
    public class ApplicationErrorResponseExample : IExamplesProvider<ProblemDetails>
    {
        public ProblemDetails GetExamples()
        {
            return new()
            {
                Detail = "There was an unhandled exception in the backend",
                Status = 500,
                Type = "https://httpstatuses.com/500",
                Title = "Internal Server Error"
            };
        }
    }
}
