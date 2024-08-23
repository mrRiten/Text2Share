using System.ComponentModel.DataAnnotations;

namespace UserMicroService.Core.Models
{
    public class UserLogin
    {
        [Required]
        public required string Login { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
