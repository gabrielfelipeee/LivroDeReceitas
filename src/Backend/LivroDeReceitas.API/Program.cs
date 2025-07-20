using LivroDeReceitas.API.Filters;
using LivroDeReceitas.API.Middleware;
using LivroDeReceitas.Application;
using LivroDeReceitas.Infrastructure;
using LivroDeReceitas.Infrastructure.Extensions;
using LivroDeReceitas.Infrastructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc(options => options.Filters.Add(typeof(ExceptionFilter)));


// Métodos de extensão
builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);


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

app.Run();

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
{}