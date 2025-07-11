using Azure.Storage.Blobs;
using System.IO.Compression;

class Program
{
    static async Task Main()
    {
       
        string projectPath = "/Users/arulmozhikumark/Desktop/Genspark-Training/ExpenseTracker/API"; 
        string zipFileName = $"ExpenseTracker_{DateTime.UtcNow:yyyyMMdd_HHmmss}.zip";
        string zipPath = Path.Combine(Path.GetTempPath(), zipFileName);

        string connectionString = "";
        string containerName = "codebackup";

        try
        {
            Console.WriteLine("Zipping project...");
            if (File.Exists(zipPath)) File.Delete(zipPath);
            ZipFile.CreateFromDirectory(projectPath, zipPath, CompressionLevel.Optimal, includeBaseDirectory: false);
            Console.WriteLine($" Zipped at {zipPath}");

            Console.WriteLine("Connecting to Azure Blob...");
            var blobContainerClient = new BlobContainerClient(connectionString, containerName);
            await blobContainerClient.CreateIfNotExistsAsync();

            var blobClient = blobContainerClient.GetBlobClient(zipFileName);
            await using var zipStream = File.OpenRead(zipPath);

            Console.WriteLine($"Uploading {zipFileName}...");
            await blobClient.UploadAsync(zipStream, overwrite: true);
            Console.WriteLine(" Upload complete.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($" Error: {ex.Message}");
        }
    }
}
