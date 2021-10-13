using System;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeUrgenta.Common.Extensions
{

    public static class ControllerExtensions
    {

        public static Microsoft.AspNetCore.Mvc.ActionResult ToActionResult<T>(this Result<T, ValidationResult> result)
        {
            if (result.IsSuccess)
            {
                if (result.Value is Unit)
                {
                    return new NoContentResult();
                }

                return new OkObjectResult(result.Value);
            }

            switch (result.Error)
            {
                case GenericValidationError _:
                    return new BadRequestResult();

                case DetailedValidationError error:
                    var problemDetails = error.ToValidationProblemDetails();

                    return new ObjectResult(problemDetails)
                    {
                        StatusCode = problemDetails.Status
                    };

                default:
                    throw new ArgumentException($"Unsupported validation result {result.Error.GetType()}");
            }
        }

        private static ValidationProblemDetails ToValidationProblemDetails(this DetailedValidationError validationResult)
        {
            var problemDetails = new ValidationProblemDetails
            {
                Detail = "See errors for additional information",
                Status = StatusCodes.Status400BadRequest
            };

            foreach (var message in validationResult.Messages)
            {
                problemDetails.Errors.Add(message.Key, new []{message.Value});
            }

            return problemDetails;
        }

    }
}