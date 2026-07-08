using Microsoft.AspNetCore.Http;

namespace LivroDeReceitas.Comunication.Requests
{
    public class RequestRegisterRecipeFormData : RequestRecipeJson
    {
        public IFormFile? Image { get; set; }
    }
}
