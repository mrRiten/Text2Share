using System.ComponentModel.DataAnnotations;

namespace LikeMicroService.Core.Models
{
    public class Like
    {
        [Key]
        public int IdLike { get; set; }

        [Required]
        public required int UserId { get; set; }

        [Required]
        public required int TextId { get; set; }
    }
}
