using AutoMapper;
using LivroDeReceitas.Application.Services.AutoMapper;
using LivroDeReceitas.Application.Services.Cryptography;
using LivroDeReceitas.Application.UseCases.User.Register;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using LivroDeReceitas.Application.UseCases.Login.DoLogin;

namespace LivroDeReceitas.Application
{

    // Classe estática que vai conter o método de extensão. Em C#, métodos de extensão devem ser em classes estáticas.
    public static class DependencyInjectionExtension
    {
        // this está dizendo ao compilador que o método AddApplication é um método de extensão para a interface IServiceCollection.
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            AddAutoMapper(services);
            AddUseCases(services);
            AddPasswordEncrypter(services, configuration);
        }
        private static void AddAutoMapper(IServiceCollection services)
        {
            var autoMapper = new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapping());
            }).CreateMapper();

            services.AddScoped(option => autoMapper);
        }

        private static void AddUseCases(IServiceCollection services)
        {
            services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        }

        private static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
        {
            var additionalKey = configuration.GetValue<string>("Settings:Password:additionalKey");
            services.AddScoped(option => new PasswordEncrypter(additionalKey!));
        }
    }
}
