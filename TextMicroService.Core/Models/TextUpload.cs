using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace TextMicroService.Core.Models
{
    public class TextUpload
    {
        [Required]
        [NotNull]
        public required string Data { get; set; }

        [Required]
        [NotNull]
        public int UserId { get; set; }

        [DefaultValue(false)]
        public bool IsPublic { get; set; }
    }
}
