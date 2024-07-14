using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace UserMicroService.Core.Models
{
    public class UserUpload
    {
        [Required]
        [StringLength(128)]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public required string UserEmail { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
