using APLTechChallenge.Models;

namespace APLTechChallenge.Services
{
    public interface IImageService
    {
        // TODO -- change to UploadFileToAzure

        Task<bool> UploadImageToAzure(IFormFile file);
        Task<bool> PostFileAsync(IFormFile file);
    }
}
