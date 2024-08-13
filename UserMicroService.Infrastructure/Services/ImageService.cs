using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using UserMicroService.Application.Services;

namespace UserMicroService.Infrastructure.Services
{
    public class ImageService(IWebHostEnvironment webHost) : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHost;

        public async Task<string> UploadUserImageAsync(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                throw new ArgumentException("No file upload");
            }

            // Check type if image
            var supportedTypes = new[] { "jpg", "jpeg", "png", "gif" };
            var fileExt = Path.GetExtension(image.FileName).Substring(1).ToLower();
            if (!supportedTypes.Contains(fileExt))
            {
                throw new ArgumentException("Invalid file type. Only jpg, jpeg, png, and gif files are allowed.");
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);   // Unique ID for image

            // Add file to folder
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);

            if (!Directory.Exists(Path.Combine(_webHostEnvironment.WebRootPath, "uploads")))
            {
                Directory.CreateDirectory(Path.Combine(_webHostEnvironment.WebRootPath, "uploads"));
            }

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return Path.Combine("uploads", fileName);   // path to file
        }
    }
}
