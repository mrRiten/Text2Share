﻿using System.ComponentModel.DataAnnotations;

namespace EmailMicroService.Core.Models
{
    public class Email
    {
        [Key]
        public int IdEmail { get; set; }

        [Required]
        public required string UserEmail { get; set; }

        [Required]
        public required string Data { get; set; }
    }
}
