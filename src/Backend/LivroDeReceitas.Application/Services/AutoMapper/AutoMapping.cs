using AutoMapper;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;
using LivroDeReceitas.Domain.Entities;

namespace LivroDeReceitas.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            RequestToDomain();
            DomainToResponse();
        }

        private void RequestToDomain()
        {
            //           Fonte dos dados      |  Destino
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(user => user.Password, opt => opt.Ignore()); // Vai ignorar o mapeamento de Password (pois será criptografada)
        }

        private void DomainToResponse()
        {
            //       Fonte dos dados      |  Destino
            CreateMap<User, ResponseUserProfileJson>();
        }
    }
}
