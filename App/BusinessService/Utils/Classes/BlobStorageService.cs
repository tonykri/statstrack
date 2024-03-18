using BusinessService.Dto;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace BusinessService.Utils;


public class BlobStorageService : IBlobStorageService
{
    private readonly CloudStorageAccount _storageAccount;
    private readonly CloudBlobClient _blobClient;
    private readonly IConfiguration _configuration;
    private readonly string containerName = "businessphotos";

    public BlobStorageService(IConfiguration configuration)
    {
        _configuration = configuration;
        string? connectionString = _configuration["BlobStorageConnection"];
        _storageAccount = CloudStorageAccount.Parse(connectionString);
        _blobClient = _storageAccount.CreateCloudBlobClient();
    }

    public async Task UploadBlobAsync(string blobName, IFormFile file)
    {
        var container = _blobClient.GetContainerReference(containerName);
        await container.CreateIfNotExistsAsync();
        var blob = container.GetBlockBlobReference(blobName);
        using (var stream = file.OpenReadStream())
        {
            await blob.UploadFromStreamAsync(stream);
        }
    }

    public async Task<ImageDto> GetBlobAsync(string blobName)
    {
        var container = _blobClient.GetContainerReference(containerName);
        var blob = container.GetBlobReference(blobName);
        var stream = new MemoryStream();
        await blob.DownloadToStreamAsync(stream);
        stream.Position = 0;
        return new ImageDto(stream, blob.Properties.ContentType, blobName);
    }

    public async Task DeleteBlobAsync(string blobName)
    {
        var container = _blobClient.GetContainerReference(containerName);
        var blob = container.GetBlockBlobReference(blobName);
        await blob.DeleteIfExistsAsync();
    }

    public async Task UpdateBlobAsync(string blobName, IFormFile file)
    {
        await DeleteBlobAsync(blobName);
        await UploadBlobAsync(blobName, file);
    }
}