using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DeUrgenta.Common.Mappers
{
    public interface IResultMapper
    {
        Task<ActionResult<T>> MapToActionResult<T>(Result<T, ValidationResult> result, CancellationToken cancellationToken);
        Task<ActionResult> MapToActionResult(Result<Unit, ValidationResult> result, CancellationToken cancellationToken);
    }
}