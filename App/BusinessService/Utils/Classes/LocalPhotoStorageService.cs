namespace BusinessService.Utils;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using BusinessService.Dto;

public class LocalPhotoStorageService : IPhotoStorageService
{
    private readonly string _storageBasePath;
    private readonly ILogger<LocalPhotoStorageService> _logger;

    public LocalPhotoStorageService(IConfiguration configuration, ILogger<LocalPhotoStorageService> logger)
    {
        _storageBasePath = configuration["PhotoStorage:BasePath"] 
            ?? Path.Combine(Directory.GetCurrentDirectory(), "PhotoStorage");
        _logger = logger;
        
        if (!Directory.Exists(_storageBasePath))
        {
            Directory.CreateDirectory(_storageBasePath);
        }
    }

    public async Task<string> UploadPhotoAsync(string fileName, IFormFile file)
    {
        try
        {
            var filePath = GetFilePath(fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);
            return filePath;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading photo {FileName}", fileName);
            throw new Exception(ExceptionMessages.NOT_VALID);
        }
    }

    public async Task<ImageDto> GetPhotoAsync(string fileName)
    {
        var filePath = GetFilePath(fileName);
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Photo {fileName} not found");
            
        var data = await File.ReadAllBytesAsync(filePath);
        var contentType = GetContentType(fileName);
        return new ImageDto(data, contentType, fileName);
    }
        
    private string GetContentType(string fileName) => 
        Path.GetExtension(fileName).ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };

    public async Task DeletePhotoAsync(string fileName)
    {
        var filePath = GetFilePath(fileName);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Photo {fileName} not found");
        }

        try
        {
            File.Delete(filePath);
            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting photo {FileName}", fileName);
            throw new Exception(ExceptionMessages.NOT_VALID);
        }
    }

    public async Task<string> UpdatePhotoAsync(string fileName, IFormFile newFile)
    {
        await DeletePhotoAsync(fileName);
        return await UploadPhotoAsync(fileName, newFile);
    }

    private string GetFilePath(string fileName)
    {
        // Sanitize fileName to prevent directory traversal
        fileName = Path.GetFileName(fileName);
        return Path.Combine(_storageBasePath, fileName);
    }
}
