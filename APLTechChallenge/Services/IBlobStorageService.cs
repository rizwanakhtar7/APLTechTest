namespace APLTechChallenge.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadImageAsync(Stream imageStream, string imageName, string fileExtension);
    }
}
