using System;

namespace DeUrgenta.Courses.Api.Models
{
    public sealed record CourseModel
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string IssuingAuthority { get; init; }
        public DateTime ExpirationDate { get; init; }
    }
}
