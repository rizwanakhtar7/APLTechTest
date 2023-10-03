using APLTechChallenge.Helpers;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

namespace APLTechChallenge.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _blobContainerName;
        public BlobStorageService(BlobServiceClient blobServiceClient, IOptions<AzureDTO> azureConfiguration)
        {
            _blobServiceClient = blobServiceClient;
            _blobContainerName = azureConfiguration.Value.Container;
        }
        public async Task<string> UploadImageAsync(Stream imageStream, string imageName, string fileExtension)
        {
            BlobClient blobClient = GetBlobDetails(imageName);

            string contentType = GetCorrectFileExtension(fileExtension);

            var blobHttpHeaders = new BlobHttpHeaders { ContentType = contentType };

            await blobClient.UploadAsync(imageStream, blobHttpHeaders);

            return blobClient.Uri.ToString();
        }

        private BlobClient GetBlobDetails(string imageName)
        {
            var getContainerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);

            if (getContainerClient == null)
            {
                throw new Exception("No Container found in azure..");
            }

            var blobClient = getContainerClient.GetBlobClient(imageName);
            return blobClient;
        }

        private static string GetCorrectFileExtension(string fileExtension)
        {
            string contentType;

            switch (fileExtension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    contentType = "image/jpeg";
                    break;
                case ".png":
                    contentType = "image/png";
                    break;
                default:
                    contentType = "application/octet-stream";
                    break;
            }

            return contentType;
        }
    }
}
