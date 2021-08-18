using System.Collections.Generic;

namespace DeUrgenta.User.Api.Models
{
    public record LoginResponse
    {
        public string Token { get; init; }
        public string LastName { get; init; }
        public string FirstName { get; init; }
        public bool Success { get; init; }
        public List<string> Errors { get; init; } = new List<string>();
    }
}
