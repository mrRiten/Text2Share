using Microsoft.AspNetCore.Http;

namespace UserMicroService.Application.Services
{
    public interface IImageService
    {
        public Task<string> UploadUserImageAsync(IFormFile image);
    }
}
