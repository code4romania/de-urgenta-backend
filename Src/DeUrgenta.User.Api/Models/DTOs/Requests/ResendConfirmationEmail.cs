﻿using System.ComponentModel.DataAnnotations;

namespace DeUrgenta.User.Api.Models.DTOs.Requests
{
    public class ResendConfirmationEmail
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}