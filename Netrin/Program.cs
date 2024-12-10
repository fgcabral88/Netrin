using Microsoft.OpenApi.Models;
using Netrin.Infraestructure.Data.Context;
using Netrin.Infraestructure.IoC;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Netrin",
        Version = "v1",
        Description = "Autor: Felipe Gabriel Cabral - Netrin - .NET 8 - SQLServer - Swagger",
    });

    options.EnableAnnotations();
});

// Registrar depend�ncias IoC
builder.Services.AdicionarDependencias();

// Adiciona a string de conex�o ao cont�iner de servi�os
builder.Services.AddScoped<SqlDbContext>(sp =>
    new SqlDbContext(builder.Configuration.GetConnectionString("SqlServer")!));

// Serilog
builder.Host.UseSerilog();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // L� as configura��es do appsettings.json
    .Enrich.FromLogContext() // Adiciona informa��es de contexto
    .WriteTo.Console() // Exibe no console
    .WriteTo.MongoDB(
        databaseUrl: builder.Configuration.GetConnectionString("MongoDB")!,
        collectionName: "Logs") // Salva no MongoDB
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
}

// Middleware de Rate Limiting - Limite de 3 requisi��es por minuto
app.UseMiddleware<RateLimitingMiddleware>(3, TimeSpan.FromMinutes(1)); 

app.UseReDoc(options =>
{
    options.DocumentTitle = "Netrin - Redoc";
    options.SpecUrl = "/swagger/v1/swagger.json";
    options.RoutePrefix = "redoc";
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
