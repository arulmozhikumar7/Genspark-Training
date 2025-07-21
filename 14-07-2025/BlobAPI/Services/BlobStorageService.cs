using Azure.Storage.Blobs;

namespace BlobAPI.Services
{
    public class BlobStorageService
    {
        private BlobContainerClient _containerClient;

        public BlobStorageService(KeyVaultService keyVaultService)
        {
            InitAsync(keyVaultService).Wait(); 
        }

        private async Task InitAsync(KeyVaultService keyVaultService)
        {
            var sasUrl = await keyVaultService.GetSecretAsync("ContainerSasUrl");
            _containerClient = new BlobContainerClient(new Uri(sasUrl));
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
