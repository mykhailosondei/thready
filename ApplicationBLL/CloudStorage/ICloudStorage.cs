using Microsoft.AspNetCore.Http;

namespace ApplicationBLL.CloudStorage;

public interface ICloudStorage
{
    string Log();
    Task<string> UploadFileAsync(IFormFile file, string fileName, string contentType);
    Task DeleteFileAsync(string fileName);
}