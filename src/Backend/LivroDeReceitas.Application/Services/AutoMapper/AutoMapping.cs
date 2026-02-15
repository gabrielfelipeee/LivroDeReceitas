using AutoMapper;
using LivroDeReceitas.Comunication.Requests;
using LivroDeReceitas.Comunication.Responses;
using LivroDeReceitas.Domain.Entities;
using Sqids;

namespace LivroDeReceitas.Application.Services.AutoMapper
{
    public class AutoMapping : Profile
    {
        private readonly SqidsEncoder<long> _idEncoder;
        public AutoMapping(SqidsEncoder<long> idEncoder)
        {
            _idEncoder = idEncoder;

            RequestToDomain();
            DomainToResponse();
        }

        private void RequestToDomain()
        {
            //           Fonte dos dados      |  Destino
            CreateMap<RequestRegisterUserJson, User>()
                .ForMember(user => user.Password, opt => opt.Ignore()); // Vai ignorar o mapeamento de Password (pois será criptografada)

            CreateMap<RequestRecipeJson, Recipe>()
                .ForMember(recipe => recipe.Instructions, opt => opt.Ignore())
                .ForMember(recipe => recipe.Ingredients, opt => opt.MapFrom(source => source.Ingredients.Distinct())) // Ignora os ingredientes repetidos
                .ForMember(recipe => recipe.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Distinct()));

            // Mapeia string de 'IList<string> Ingredients' (Request) para propriedade Item da classe Ingredient
            CreateMap<string, Ingredient>()
                .ForMember(dest => dest.Item, opt => opt.MapFrom(source => source));

            CreateMap<Comunication.Enums.DishType, DishType>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source));

            CreateMap<RequestInstructionJson, Instruction>();
        }

        private void DomainToResponse()
        {
            //       Fonte dos dados      |  Destino
            CreateMap<User, ResponseUserProfileJson>();

            CreateMap<Recipe, ResponseRegisteredRecipeJson>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)));

            CreateMap<Recipe, ResponseShortRecipeJson>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)))
                .ForMember(dest => dest.AmountIngredients, opt => opt.MapFrom(source => source.Ingredients.Count));


            CreateMap<Recipe, ResponseRecipeJson>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)))
                .ForMember(dest => dest.DishTypes, opt => opt.MapFrom(source => source.DishTypes.Select(r => r.Type)));
            CreateMap<Ingredient, ResponseIngredientJson>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)));
            CreateMap<Instruction, ResponseInstructionJson>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => _idEncoder.Encode(source.Id)));
        }
    }
}
