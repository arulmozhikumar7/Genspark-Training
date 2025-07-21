using Azure.Storage.Blobs;

namespace BlobAPI.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService(KeyVaultService keyVaultService)
        {
            _containerClient = InitAsync(keyVaultService).GetAwaiter().GetResult();
        }

        private async Task<BlobContainerClient> InitAsync(KeyVaultService keyVaultService)
        {
            var connectionString = await keyVaultService.GetSecretAsync("AzureBlobConnectionString");
            var containerName = "demo"; 

            return new BlobContainerClient(connectionString, containerName);
        }

        public async Task UploadFile(Stream fileStream, string fileName)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true);
        }

        public async Task<Stream?> DownloadFile(string fileName)
        {
            var blobClient = _containerClient.GetBlobClient(fileName);
            if (await blobClient.ExistsAsync())
            {
                var downloadInfo = await blobClient.DownloadStreamingAsync();
                return downloadInfo.Value.Content;
            }
            return null;
        }
    }
}
