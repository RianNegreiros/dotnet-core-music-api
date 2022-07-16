using Azure.Storage.Blobs;

namespace MusicApi.Helpers;

public static class FileHelper
{
    public static async Task<string> UploadImage(IFormFile file)
    {
        string connectionString = "DefaultEndpointsProtocol=https;AccountName=aspnetcoremusicapi;AccountKey=lp2U1BGdlu8gLcIjbDerGL9IHBafkcGe9H1MeeQsCz4PUfFtxLsbsCxlvVspQfj893SGSSi72dXQ+AStFYm9ag==;EndpointSuffix=core.windows.net";
        string containerName = "aspnetcoremusicapi";

        BlobContainerClient blobContainerClient = new BlobContainerClient(connectionString, containerName);
        BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);
        MemoryStream memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        memoryStream.Position = 0;
        await blobClient.UploadAsync(memoryStream);
        return blobClient.Uri.AbsoluteUri;
    }
}