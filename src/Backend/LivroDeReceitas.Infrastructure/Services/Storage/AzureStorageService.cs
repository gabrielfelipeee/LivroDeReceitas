using Azure.Storage.Blobs;
using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Domain.Services.Storage;

namespace LivroDeReceitas.Infrastructure.Services.Storage
{
    public class AzureStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public AzureStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task Upload(User user, Stream file, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(user.UserIdentifier.ToString());
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(file, overwrite: true);
        }
    }
}
