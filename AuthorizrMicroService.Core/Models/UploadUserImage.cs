using Microsoft.AspNetCore.Http;

namespace UserMicroService.Core.Models
{
    public class UploadUserImage
    {
        public required IFormFile Image { get; set; }
    }
}
