using System;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Configure o DbContext apontando para seu SQL Server.
// Defina a connection string em `appsettings.Development.json` usando a chave `ConnectionStrings:ConexaoPadrao` ou
// defina a variável de ambiente `TRILHA_CONNECTION` (a factory de design-time usa essa variável quando presente).
// Exemplo de connection string:
// Server=YOUR_SERVER;Database=TrilhaApiDesafioDb;User Id=sa;Password=YourPassword;TrustServerCertificate=True;
builder.Services.AddDbContext<OrganizadorContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBI", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Data Analysis API",
        Version = "v1",
        Description = "API REST para análises de dados, integrável com Power BI, Tableau e outras plataformas de visualização.",
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Apply migrations and seed data (if needed)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrganizadorContext>();
    db.Database.Migrate();

    if (!db.Tarefas.Any())
    {
        db.Tarefas.AddRange(
            new Tarefa { Titulo = "Tarefa Exemplo 1", Descricao = "Tarefa gerada automaticamente", Data = DateTime.UtcNow.Date, Status = EnumStatusTarefa.Pendente },
            new Tarefa { Titulo = "Tarefa Exemplo 2", Descricao = "Tarefa gerada automaticamente", Data = DateTime.UtcNow.Date.AddDays(-1), Status = EnumStatusTarefa.Finalizado }
        );
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Data Analysis API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseCors("AllowBI");

app.UseAuthorization();

app.MapControllers();

app.Run();
