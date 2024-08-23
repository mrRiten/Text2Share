using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using UserMicroService.Application.Services;

namespace UserMicroService.Infrastructure.Services
{
    public class ImageService(IWebHostEnvironment webHostEnvironment) : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;
        private readonly string[] _supportedTypes = ["jpg", "jpeg", "png", "gif"];

        public async Task<string> UploadUserImageAsync(IFormFile image)
        {
            ValidateImage(image);
            var fileName = GenerateUniqueFileName(image);
            var filePath = GetFilePath(fileName);
            await SaveImageAsync(image, filePath);

            return Path.Combine("uploads", fileName); // Return the relative path to the file
        }

        private void ValidateImage(IFormFile image)
        {
            if (image == null || image.Length == 0)
            {
                throw new InvalidOperationException("No file uploaded.");
            }

            var fileExt = Path.GetExtension(image.FileName)?.Substring(1).ToLower();
            if (fileExt == null || !_supportedTypes.Contains(fileExt))
            {
                throw new InvalidOperationException("Invalid file type. Only jpg, jpeg, png, and gif files are allowed.");
            }
        }

        private string GenerateUniqueFileName(IFormFile image)
        {
            return Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
        }

        private string GetFilePath(string fileName)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            EnsureUploadsFolderExists(uploadsFolder);
            return Path.Combine(uploadsFolder, fileName);
        }

        private void EnsureUploadsFolderExists(string uploadsFolder)
        {
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
        }

        private async Task SaveImageAsync(IFormFile image, string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
        }
    }
}
