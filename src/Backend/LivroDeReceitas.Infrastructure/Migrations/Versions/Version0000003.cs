using FluentMigrator;

namespace LivroDeReceitas.Infrastructure.Migrations.Versions
{
    // Versão e descrição do que a versão faz
    [Migration(DatabaseVersions.IMAGES_FOR_RECIPES, "Adiciona coluna na tabela de receitas para imagem")]
    public class Version0000003 : VersionBase
    {
        public override void Up()
        {
            Alter.Table("Recipes").AddColumn("ImageIdentifier").AsString().Nullable();
        }
    }
}
