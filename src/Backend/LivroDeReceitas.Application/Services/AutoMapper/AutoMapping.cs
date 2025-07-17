using AutoMapper;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Domain.Entities;

namespace LivroDeReceitas.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToDomain();
        }

        private void RequestToDomain()
        {
            //           Fonte dos dados      |  Destino
            CreateMap<RequestRegisterUserJson, UserEntity>()
                .ForMember(user => user.Password, opt => opt.Ignore()); // Vai ignorar o mapeamento de Password (pois será criptografada)
        }
    }
}
