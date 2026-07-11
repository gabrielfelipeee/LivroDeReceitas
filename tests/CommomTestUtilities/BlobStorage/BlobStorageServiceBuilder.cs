using Bogus;
using LivroDeReceitas.Domain.Entities;
using LivroDeReceitas.Domain.Services.Storage;
using Moq;

namespace CommomTestUtilities.BlobStorage
{
    public class BlobStorageServiceBuilder
    {
        private readonly Mock<IBlobStorageService> _mock;
        public BlobStorageServiceBuilder() => _mock = new Mock<IBlobStorageService>();

        public BlobStorageServiceBuilder GetFileUrl(User user, string? fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return this;

            var imageUrl = new Faker().Image.LoremFlickrUrl();

            _mock.Setup(blobStorage => blobStorage.GetFileUrl(user, fileName)).ReturnsAsync(imageUrl);

            return this;
        }

        public BlobStorageServiceBuilder GetFileUrl(User user, IList<Recipe> recipes)
        {
            foreach (var recipe in recipes)
                GetFileUrl(user, recipe.ImageIdentifier);

            return this;
        }

        public IBlobStorageService Build() => _mock.Object;
    }
}
