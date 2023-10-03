using APLTechChallenge.Helpers;
using APLTechChallenge.Models;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.Drawing;

namespace APLTechChallenge.Services
{
    public class ImageService : IImageService
    {
        private readonly AzureDTO _azureConfiguration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IBlobStorageService _blobStorageService;

        public ImageService(IOptions<AzureDTO> azureConfiguration, IWebHostEnvironment webHostEnvironment, IBlobStorageService blobStorageService)
        {
            _azureConfiguration = azureConfiguration.Value;
            _webHostEnvironment = webHostEnvironment;
            _blobStorageService = blobStorageService;
        }

        public async Task<bool> PostFileAsync(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    ImageValidator.CheckExtension(file);
                    ImageValidator.CheckImageDimensions(file);

                    string filePath = CreateLocalImgDirectory(file);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }

        public async Task<bool> UploadImageToAzure(IFormFile file)
        {
            try
            {
                if (file.Length > 0)
                {
                    ImageValidator.CheckExtension(file);
                    ImageValidator.CheckImageDimensions(file);
                    
                    using (var fileUploadStream = new MemoryStream())
                    {
                        await file.CopyToAsync(fileUploadStream);
                        fileUploadStream.Position = 0;

                        await _blobStorageService.UploadImageAsync(fileUploadStream, file.FileName, Path.GetExtension(file.FileName));
                      
                    }
                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("File Copy Failed", ex);
            }
        }

        private string CreateLocalImgDirectory(IFormFile file)
        {
            var wwwrootPath = _webHostEnvironment.WebRootPath;

            var uploadFolderPath = Path.Combine(wwwrootPath, "UploadedFiles");

            if (!Directory.Exists(uploadFolderPath))
            {
                Directory.CreateDirectory(uploadFolderPath);
            }

            var filePath = Path.Combine(uploadFolderPath, file.FileName);
            return filePath;
        }
    }
}
