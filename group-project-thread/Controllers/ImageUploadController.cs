using ApplicationBLL.CloudStorage;
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
    public async Task<IActionResult> UploadImage(IFormFile file)
    {
        _logger.LogWarning(_cloudStorage.Log());
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file was uploaded");
        }
        
        var contentType = file.ContentType;
        
        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        
        var result = await _cloudStorage.UploadFileAsync(file, fileName, contentType);
        if (result == null)
        {
            return BadRequest("Something went wrong");
        }
        else
        {
            return Ok(result);
        }
    }
}