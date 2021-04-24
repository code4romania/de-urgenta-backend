using System.Threading.Tasks;
using MediatR;

namespace DeUrgenta.Group.Api.Validators
{
    public interface IValidateRequest<in T> where T : IBaseRequest
    {
        Task<bool> IsValidAsync(T request);
    }
}