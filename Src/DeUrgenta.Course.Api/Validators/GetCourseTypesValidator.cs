﻿using System.Threading.Tasks;
using DeUrgenta.Courses.Api.Queries;
using DeUrgenta.Common.Validation;
using DeUrgenta.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeUrgenta.Courses.Api.Validators
{
    public class GetCourseTypesValidator : IValidateRequest<GetCourseTypes>
    {
        private readonly DeUrgentaContext _context;

        public GetCourseTypesValidator(DeUrgentaContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidAsync(GetCourseTypes request)
        {
            return true;
        }
    }
}