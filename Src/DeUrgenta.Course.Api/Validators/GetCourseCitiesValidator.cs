using System.Threading.Tasks;
using DeUrgenta.Courses.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Courses.Api.Validators
{
    public class GetCourseCitiesValidator : IValidateRequest<GetCourseCities>
    {
        private readonly DeUrgentaContext _context;

        public GetCourseCitiesValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(GetCourseCities request)
        {
            return true;
        }
    }
}
