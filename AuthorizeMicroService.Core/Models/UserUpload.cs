using AuthorizeMicroService.Core.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AuthorizeMicroService.Core.Models
{
    public class UserUpload
    {
        [Required]
        [StringLength(128)]
        [UniqueUserName]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        [UniqueEmail]
        [StringLength(128)]
        public required string UserEmail { get; set; }

        [Required]
        [StringLength(32)]
        public required string Password { get; set; }
    }
}
