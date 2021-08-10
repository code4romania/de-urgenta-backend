using System;
using DeUrgenta.Courses.Api.Models;
using MediatR;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;

namespace DeUrgenta.Courses.Api.Queries
{
    public class GetCourseTypes : IRequest<Result<IImmutableList<CourseTypeModel>>>
    {
        public GetCourseTypes()
        {
        }
    }
}
