using System.ComponentModel.DataAnnotations.Schema;

namespace LivroDeReceitas.Domain.Entities
{
    [Table("DishTypes")]
    public class DishType : EntityBase
    {
        public Enums.DishType Type { get; set; }
        public long RecipeId { get; set; }
    }
}
