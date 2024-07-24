﻿using System.ComponentModel.DataAnnotations;

namespace UserMicroService.Core.Models
{
    public class UserUpload
    {
        [Required]
        [StringLength(128)]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(128)]
        public required string UserEmail { get; set; }

        [Required]
        [StringLength(32)]
        public required string Password { get; set; }
    }
}
