using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace LikeMicroService.Core.Models
{
    public class Text
    {
        [Key]
        public int IdText { get; set; }

        [Required]
        [NotNull]
        public required string Data { get; set; }

        [Required]
        [NotNull]
        public int UserId { get; set; }

        public DateTime DateOfCreate { get; set; }

        public DateTime DateOfChange { get; set; }

        [DefaultValue(false)]
        public bool IsPublic { get; set; }

        [DefaultValue(0)]
        public int LikeCount { get; set; }

        [NotNull]
        [Required]
        public required string PrivetToken { get; set; }

        public void DeletePrivetToken()
        {
            PrivetToken = string.Empty;
        }
    }
}
