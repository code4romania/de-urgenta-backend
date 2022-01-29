using System.Collections.Generic;

namespace DeUrgenta.Common.Models.Dtos
{
    public class ActionResponse
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}