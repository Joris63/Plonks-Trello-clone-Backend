﻿using System.ComponentModel.DataAnnotations;

namespace Plonks.Auth.Models
{
    public class RegisterRequest
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
