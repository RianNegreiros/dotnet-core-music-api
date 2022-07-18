using Azure.Storage.Blobs;

namespace MusicApi.Helpers;

public static class FileHelper
{
    public static async Task<string> UploadImage(IFormFile file, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("AzureStorage");
        string containerName = "artistsimage";

        BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
        BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
        MemoryStream memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        await blobClient.UploadAsync(memoryStream);
        return blobClient.Uri.AbsoluteUri;
    }
    
    public static async Task<string> UploadFile(IFormFile file, IConfiguration configuration)
    {
        string connectionString = configuration.GetConnectionString("AzureStorage");
        string containerName = "audiofiles";

        BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
        BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
        MemoryStream memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        await blobClient.UploadAsync(memoryStream);
        return blobClient.Uri.AbsoluteUri;
    }
}