using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using LivroDeReceitas.Application.UseCases.User.Profile;
using LivroDeReceitas.Domain.Entities;
using Shouldly;

namespace UseCases.Test.User.Profile
{
    public class GetUserProfileUseCaseTest
    {
        [Fact]
        public async Task Success()
        {
            (var userEntity, var _) = UserEntityBuilder.Build();

            var useCase = CreateUseCase(userEntity);

            var result = await useCase.Execute();

            result.ShouldSatisfyAllConditions(userProfileResponse =>
            {
                userProfileResponse.ShouldNotBeNull();
                userProfileResponse.Name.ShouldBe(userEntity.Name);
                userProfileResponse.Email.ShouldBe(userEntity.Email);
            });
        }

        private static GetUserProfileUseCase CreateUseCase(LivroDeReceitas.Domain.Entities.User userEntity)
        {
            var mapper = MapperBuilder.Build();
            var loggedUser = LoggedUserBuilder.Build(userEntity);

            return new GetUserProfileUseCase(loggedUser, mapper);
        }
    }
}
