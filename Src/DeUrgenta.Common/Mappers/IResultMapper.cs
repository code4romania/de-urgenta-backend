using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using DeUrgenta.Common.Validation;
using Microsoft.AspNetCore.Mvc;

namespace DeUrgenta.Common.Mappers
{
    public interface IResultMapper
    {
        Task<ActionResult> MapToActionResult<T>(Result<T, ValidationResult> result);
    }
}