using System.ComponentModel.DataAnnotations.Schema;

namespace LivroDeReceitas.Domain.Entities
{
    [Table("Ingredients")]
    public class Ingredient : EntityBase
    {
        public string Item { get; set; } = string.Empty;
        public long RecipeId { get; set; }
    }
}
