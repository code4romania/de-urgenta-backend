using System;

namespace DeUrgenta.Group.Api.Models
{
    public sealed record GroupMemberModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsGroupAdmin { get; set; }
    }
}