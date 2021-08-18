using System.Collections.Generic;

namespace DeUrgenta.User.Api.Models.DTOs.Responses
{
    public class ActionResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}