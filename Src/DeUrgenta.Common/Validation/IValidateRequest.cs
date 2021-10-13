using System.Threading.Tasks;
using MediatR;

namespace DeUrgenta.Common.Validation
{
    public interface IValidateRequest<in T> where T : IBaseRequest
    {
        public ValidationPassedResult ValidationPass()
        {
            return new();
        }
        Task<ValidationResult> IsValidAsync(T request);
    }
}