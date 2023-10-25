using System.Net.Mime;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using group_project_thread.AppSettingsMimics;
using Microsoft.AspNetCore.Http;

namespace ApplicationBLL.CloudStorage;

public class GoogleCloudStorage : ICloudStorage
{
    private readonly GoogleCloudStorageSettings _googleCloudStorageSettings;
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;
    
    public GoogleCloudStorage(GoogleCloudStorageSettings googleCloudStorageSettings)
    {
        _googleCloudStorageSettings = googleCloudStorageSettings;
        var googleCredential = GoogleCredential.FromFile(_googleCloudStorageSettings.CredentialFile);
        _storageClient = StorageClient.Create(googleCredential);
        _bucketName = _googleCloudStorageSettings.BucketName;
    }
    
    public string Log()
    {
        return _googleCloudStorageSettings.BucketName + " " + _googleCloudStorageSettings.CredentialFile;
    }
    
    public async Task<string> UploadFileAsync(IFormFile file, string fileName, string contentType)
    {
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        stream.Position = 0;
        await _storageClient.UploadObjectAsync(_bucketName, fileName, contentType, stream);
        return "https://storage.googleapis.com/" + _bucketName + "/" + fileName;
    }
    
    public async Task DeleteFileAsync(string fileName)
    {
        await _storageClient.DeleteObjectAsync(_bucketName, fileName);
    }
    
}