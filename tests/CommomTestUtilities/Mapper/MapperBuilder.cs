using AutoMapper;
using CommomTestUtilities.IdEncryption;
using LivroDeReceitas.Application.Services.AutoMapper;


namespace CommomTestUtilities.Mapper
{
    public class MapperBuilder
    {
        public static IMapper Build()
        {
            var idEncripter = IdEncripterBuilder.Build();

            return new MapperConfiguration(options =>
            {
                options.AddProfile(new AutoMapping(idEncripter));
            }).CreateMapper();
        }
    }
}
