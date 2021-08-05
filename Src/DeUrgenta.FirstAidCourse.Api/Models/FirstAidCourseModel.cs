using System;

namespace DeUrgenta.FirstAidCourse.Api.Models
{
    public sealed record FirstAidCourseModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string IssuingAuthority { get; init; }
        public DateTime ExpirationDate { get; init; }
    }
}
