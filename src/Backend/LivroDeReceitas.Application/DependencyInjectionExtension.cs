using AutoMapper;
using LivroDeReceitas.Application.Services.AutoMapper;
using LivroDeReceitas.Application.UseCases.Login.DoLogin;
using LivroDeReceitas.Application.UseCases.Recipe.Register;
using LivroDeReceitas.Application.UseCases.User.ChangePassword;
using LivroDeReceitas.Application.UseCases.User.Profile;
using LivroDeReceitas.Application.UseCases.User.Register;
using LivroDeReceitas.Application.UseCases.User.Update;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sqids;

namespace LivroDeReceitas.Application
{

    // Classe estática que vai conter o método de extensão. Em C#, métodos de extensão devem ser em classes estáticas.
    public static class DependencyInjectionExtension
    {
        // this está dizendo ao compilador que o método AddApplication é um método de extensão para a interface IServiceCollection.
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddAutoMapper(services, configuration);
            AddUseCases(services);
        }
        private static void AddAutoMapper(IServiceCollection services, IConfiguration configuration)
        {
            var sqids = new SqidsEncoder<long>(new()
            {
                MinLength = 10, // Tamanho mínimo do Id
                Alphabet = configuration.GetValue<string>("Settings:IdCryptographyAlphabet")!
            });

            var autoMapper = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapping(sqids));
            }).CreateMapper();

            services.AddScoped(option => autoMapper);
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
            services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
            services.AddScoped<IChangePasswordUseCase, ChangePasswordUseCase>();

            services.AddScoped<IRegisterRecipeUseCase, RegisterRecipeUseCase>();
        }
    }
}
