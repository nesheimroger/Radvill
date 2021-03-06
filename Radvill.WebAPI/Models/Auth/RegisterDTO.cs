﻿using System.ComponentModel.DataAnnotations;

namespace Radvill.WebAPI.Models.Auth
{
    public class RegisterDTO
    {
        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } 
    }
}