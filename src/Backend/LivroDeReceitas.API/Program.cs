using LivroDeReceitas.API.BackgroundServices;
using LivroDeReceitas.API.Converters;
using LivroDeReceitas.API.Filters;
using LivroDeReceitas.API.Middleware;
using LivroDeReceitas.API.Token;
using LivroDeReceitas.Application;
using LivroDeReceitas.Domain.Security.Tokens;
using LivroDeReceitas.Infrastructure;
using LivroDeReceitas.Infrastructure.Extensions;
using LivroDeReceitas.Infrastructure.Migrations;
using Microsoft.OpenApi.Models;

const string AUTHENTICATION_TYPE = "Bearer";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new StringConverter()));
builder.Services.AddOpenApi();

// Adiciona e configura o Swagger para a API
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<IdsFilter>();

    // Define o esquema de segurança chamado "Bearer" (para autenticação via JWT)
    options.AddSecurityDefinition(AUTHENTICATION_TYPE, new OpenApiSecurityScheme
    {
        // Descrição que será exibida na UI do Swagger para orientar o usuário
        Description = "JWT Authorization",

        // Nome do cabeçalho onde o token deve ser enviado (Authorization)
        Name = "Authorization",

        // Informa que o token deve ser enviado no Header da requisição
        In = ParameterLocation.Header,

        // Define o tipo de esquema de segurança como ApiKey (usado aqui para tokens JWT)
        Type = SecuritySchemeType.ApiKey,

        // Esquema que será utilizado (Bearer)
        Scheme = AUTHENTICATION_TYPE
    });

    // Define os requisitos de segurança que devem ser aplicados às requisições
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            // Referência ao esquema de segurança definido acima
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    // Diz que a referência é para um SecurityScheme
                    Type = ReferenceType.SecurityScheme,

                    // O Id do esquema definido anteriormente ("Bearer")
                    Id = AUTHENTICATION_TYPE
                },

                // O esquema que será usado, aqui colocado como "oauth2"
                Scheme = "oauth2",

                // Nome do esquema
                Name = AUTHENTICATION_TYPE,

                // Onde o token será informado (no Header)
                In = ParameterLocation.Header
            },

            // Lista de escopos exigidos (nesse caso, vazia → qualquer token válido serve)
            new List<string>()
        }
    });
});

builder.Services.AddMvc(options => options.Filters.Add<ExceptionFilter>());

// Métodos de extensão
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

builder.Services.AddHostedService<DeleteUserService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

MigrateDatabase();

await app.RunAsync();

void MigrateDatabase()
{
    // Não precisa fazer migração ao fazer testes
    if (builder.Configuration.IsUnitTestEnvironment())
        return;

    var databaseType = builder.Configuration.DatabaseType();
    var connectionString = builder.Configuration.ConnectionString();
    var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider;

    DatabaseMigration.Migrate(databaseType, serviceScope, connectionString);
}

public partial class Program
{
    protected Program() { }
}
