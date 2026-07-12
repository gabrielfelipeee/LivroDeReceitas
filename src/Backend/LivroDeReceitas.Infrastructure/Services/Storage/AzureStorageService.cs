using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Domain.Extensions;
using LivroDeReceitas.Domain.Services.Storage;
using LivroDeReceitas.Domain.ValueObjects;

namespace LivroDeReceitas.Infrastructure.Services.Storage
{
    public class AzureStorageService : IBlobStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        public AzureStorageService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> GetFileUrl(User user, string fileName)
        {
            var containerName = user.UserIdentifier.ToString();

            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            var exist = await containerClient.ExistsAsync();
            if (exist.Value.IsFalse())
                return string.Empty;


            var blobClient = containerClient.GetBlobClient(fileName);
            exist = await blobClient.ExistsAsync();
            if (exist.Value.IsFalse())
                return string.Empty;

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = containerName,
                BlobName = fileName,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(LivroDeReceitasRuleConstants.MAXIMUM_IMAGE_URL_LIFETIME_IN_MINUTES),
            };
            sasBuilder.SetPermissions(BlobAccountSasPermissions.Read);

            var uri = blobClient.GenerateSasUri(sasBuilder);

            return uri.ToString();
        }

        public async Task Upload(User user, Stream file, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(user.UserIdentifier.ToString());
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(file, overwrite: true);
        }

        public async Task Delete(User user, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(user.UserIdentifier.ToString());
            var exist = await containerClient.ExistsAsync();
            if (exist.Value.IsFalse())
                return;

            await containerClient.DeleteBlobIfExistsAsync(fileName);
        }

        public async Task DeleteContainer(Guid userIdentifier)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(userIdentifier.ToString());
            await containerClient.DeleteIfExistsAsync();
        }
    }
}
