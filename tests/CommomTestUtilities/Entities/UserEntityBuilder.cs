using Bogus;
using CommomTestUtilities.Cryptography;
using LivroDeReceitas.Domain.Entities;

namespace CommomTestUtilities.Entities
{
    public class UserEntityBuilder
    {
        public static (User userEntity, string password) Build()
        {
            var passwordEncrypter = PasswordEncrypterBuilder.Build();

            var password = new Faker().Internet.Password();

            var userEntity = new Faker<User>()
                .RuleFor(user => user.Id, _ => 1)
                .RuleFor(user => user.UserIdentifier, _ => Guid.NewGuid())
                .RuleFor(user => user.Name, (faker) => faker.Person.FirstName)
                .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Email))
                .RuleFor(user => user.Password,() => passwordEncrypter.Encrypt(password));

            return (userEntity, password);
        }
    }
}
