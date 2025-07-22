using LivroDeReceitas.Comunication.Requests;
using Bogus;

namespace CommomTestUtilities.Requests
{
    public class RequestLoginJsonBuilder
    {
        public static RequestLoginJson Build()
        {
            return new Faker<RequestLoginJson>()
                .RuleFor(user => user.Email, (faker) => faker.Internet.Email())
                .RuleFor(user => user.Password, (faker) => faker.Internet.Password());
        }
    }
}
