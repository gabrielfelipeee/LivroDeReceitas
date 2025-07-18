using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace LivroDeReceitas.Infrastructure.Migrations.Versions
{
    // Versão e descrição do que a versão faz
    [Migration(DatabaseVersions.TABLE_USERS, "Criar a tabela de usuários")]
    public class Version0000001 : VersionBase
    {
        public override void Up()
        {
            CreateTable("Users")
            .WithColumn("Name").AsString(60).NotNullable()
            .WithColumn("Email").AsString(60).NotNullable()
           .WithColumn("Password").AsString(2000).NotNullable();
        }
    }
}
