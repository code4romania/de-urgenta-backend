using System;
using DeUrgenta.Courses.Api.Models;
using MediatR;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;

namespace DeUrgenta.Courses.Api.Queries
{
    public class GetCourseCities : IRequest<Result<IImmutableList<CourseCityModel>>>
    {
        public GetCourseCities()
        {
        }
    }
}
