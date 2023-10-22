using ApplicationBLL.CloudStorage;
using ApplicationCommon.DTOs.Image;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class ImageUploadController : ControllerBase
{
    private readonly ICloudStorage _cloudStorage;
    private readonly ILogger<ImageUploadController> _logger;
    
    public ImageUploadController(ICloudStorage cloudStorage, ILogger<ImageUploadController> logger)
    {
        _cloudStorage = cloudStorage;
        _logger = logger;
    }
    
    [HttpPost("upload")]
    public async Task<ImageStorageResponseDTO> UploadImage(IFormFile file)
    {
        _logger.LogWarning(_cloudStorage.Log());
        if (file == null || file.Length == 0)
        {
            throw new InvalidOperationException("File is empty");
        }
        
        var contentType = file.ContentType;
        
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        
        var result = await _cloudStorage.UploadFileAsync(file, fileName, contentType);
        return new ImageStorageResponseDTO(){Url = result};
    }

    [HttpDelete("delete")]
    public async Task DeleteImage(string fileName)
    {
        await _cloudStorage.DeleteFileAsync(fileName);
    }
}