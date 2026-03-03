using FluentMigrator;
using System.Data;

namespace LivroDeReceitas.Infrastructure.Migrations.Versions
{
    // Versão e descrição do que a versão faz
    [Migration(DatabaseVersions.TABLE_RECIPES, "Criar a tabela de receitas")]
    public class Version0000002 : VersionBase
    {
        private const string RECIPE_TABLE_NAME = "Recipes";
        public override void Up()
        {
            CreateTable(RECIPE_TABLE_NAME)
                .WithColumn("Title").AsString(250).NotNullable()
                .WithColumn("CookingTime").AsInt32().Nullable()
                .WithColumn("Difficulty").AsInt32().Nullable()

                // Foreign Key -> Parametros do método ForeignKey: (nome da FK, exatamente o nome da tabela pai, exatamente o nome da coluna da tabela pai)
                .WithColumn("UserId").AsInt64().NotNullable()
                    .ForeignKey("FK_Recipe_User_Id", "Users", "Id");


            CreateTable("Ingredients")
                .WithColumn("Item").AsString(250).NotNullable()
                .WithColumn("RecipeId").AsInt64().NotNullable()
                    .ForeignKey("FK_Ingredient_Recipe_Id", RECIPE_TABLE_NAME, "Id")
                    .OnDelete(Rule.Cascade); // Excluir em cascata

            CreateTable("Instructions")
                .WithColumn("Step").AsInt32().NotNullable()
                .WithColumn("Text").AsString(2000).NotNullable()
                .WithColumn("RecipeId").AsInt64().NotNullable()
                    .ForeignKey("FK_Instruction_Recipe_Id", RECIPE_TABLE_NAME, "Id")
                    .OnDelete(Rule.Cascade);

            CreateTable("DishTypes")
                .WithColumn("Type").AsInt32().NotNullable()
                .WithColumn("RecipeId").AsInt64().NotNullable()
                    .ForeignKey("FK_DishType_Recipe_Id", RECIPE_TABLE_NAME, "Id")
                    .OnDelete(Rule.Cascade);
        }
    }
}
