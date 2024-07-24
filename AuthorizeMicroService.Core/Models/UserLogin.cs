using System.ComponentModel.DataAnnotations;

namespace AuthorizeMicroService.Core.Models
{
    public class UserLogin
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
