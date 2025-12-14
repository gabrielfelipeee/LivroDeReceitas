using LivroDeReceitas.Comunication.Requests;
using Bogus;

namespace CommomTestUtilities.Requests
{
    public class RequestChangePasswordJsonBuilder
    {
        public static RequestChangePasswordJson Build(int passwordLength = 10)
        {
            return new Faker<RequestChangePasswordJson>()
                .RuleFor(user => user.Password, (f) => f.Internet.Password())
                .RuleFor(user => user.NewPassword, (f, user) => f.Internet.Password(passwordLength));
        }
    }
}
