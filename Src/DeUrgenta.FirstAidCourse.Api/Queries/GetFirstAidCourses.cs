using System;
using DeUrgenta.FirstAidCourse.Api.Models;
using MediatR;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;

namespace DeUrgenta.FirstAidCourse.Api.Queries
{
    public class GetFirstAidCourses : IRequest<Result<IImmutableList<FirstAidCourseModel>>>
    {
        public string UserSub { get; }

        public GetFirstAidCourses(string userSub)
        {
            UserSub = userSub;
        }
    }
}
