using System;
using DeUrgenta.Courses.Api.Models;
using MediatR;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;

namespace DeUrgenta.Courses.Api.Queries
{
    public class GetCourses : IRequest<Result<IImmutableList<CourseModel>>>
    {
        public string UserSub { get; }

        public GetCourses(string userSub)
        {
            UserSub = userSub;
        }
    }
}
