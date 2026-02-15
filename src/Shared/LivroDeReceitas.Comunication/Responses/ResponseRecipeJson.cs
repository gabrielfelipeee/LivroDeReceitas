using LivroDeReceitas.Comunication.Enums;

namespace LivroDeReceitas.Comunication.Responses
{
    public class ResponseRecipeJson
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public IList<ResponseIngredientJson> Ingredients { get; set; } = [];
        public IList<ResponseInstructionJson> Instructions { get; set; } = [];
        public IList<DishType> DishTypes { get; set; } = [];
        public CookingTime? CookingTime { get; set; }
        public Difficulty? Difficulty { get; set; }
    }
}
