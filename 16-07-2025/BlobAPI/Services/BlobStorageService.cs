using Azure.Storage.Blobs;

namespace BlobAPI.Services
{
    public class BlobStorageService
    {
        private BlobContainerClient _containerClient;
        private readonly KeyVaultService _keyVaultService;

        public BlobStorageService(KeyVaultService keyVaultService)
        {
            _keyVaultService = keyVaultService;
            InitAsync(_keyVaultService).Wait(); 
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
            string functionUrl = $"https://aruldotnetfunction.azurewebsites.net/api/generate-sas/{fileName}?code=GYvf_L06aOtENao5O6ZkcdxY-jQAGoSciP_7iMYoaP65AzFu1TL6Rg==";
            using var httpClient = new HttpClient();
            var functionResponse = await httpClient.GetAsync(functionUrl);

            if (!functionResponse.IsSuccessStatusCode)
                throw new Exception("Failed to generate SAS for blob");

            // Fix here: use the field _keyVaultService
            string sasUrl = await _keyVaultService.GetSecretAsync("arulblob");

            var blobClient = new BlobClient(new Uri(sasUrl));
            if (await blobClient.ExistsAsync())
            {
                var downloadInfo = await blobClient.DownloadStreamingAsync();
                return downloadInfo.Value.Content;
            }

            return null;
        }
    }
}
