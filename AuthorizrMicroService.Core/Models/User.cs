using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using UserMicroService.Core.Attributes;

namespace UserMicroService.Core.Models
{
    public class User
    {
        [Key]
        public int IdUser { get; set; }

        [Required]
        [StringLength(128)]
        [UniqueUserName]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        [UniqueEmail]
        public required string UserEmail { get; set; }

        [Required]
        public required string Password { get; set; }

        [StringLength(128)]
        public string? UserImagePath { get; set; }

        public DateTime DateOfRegister { get; set; }

        public DateTime LastLoginDate { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool IsEmailConfirmed { get; set; }

        [Required]
        [StringLength(256)]
        public required string ConfirmToken { get; set; }
    }
}
