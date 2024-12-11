using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Netrin.Application.Helpers;
using Netrin.Infraestructure.Data.Context;
using Netrin.Infraestructure.IoC;
using Serilog;
using System.Text;
using static Netrin.Application.Helpers.JwtTokenHelper;

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
        Description = "Autor: Felipe Gabriel Cabral - Netrin - C# .NET 8 - Docker - JWT - SQLServer - MongoDB - Serilog - Rate Limit - IoC - AutoMapper - FluentValidation - Middlewares - Filters - XUnit - Swagger - ReDoc",
    });

    // Configuração do esquema de autenticação
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT no formato: Bearer {seu token}"
    });

    // Configuração para aplicar segurança globalmente
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    options.EnableAnnotations();
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
    };
});

builder.Services.AddSingleton<JwtTokenHelper>();

builder.Services.AddAuthorization();

// Registrar dependências IoC
builder.Services.AdicionarDependencias();

// Adiciona a string de conexão ao contêiner de serviços
builder.Services.AddScoped<SqlDbContext>(sp =>
    new SqlDbContext(builder.Configuration.GetConnectionString("SqlServer")!));

// Serilog
builder.Host.UseSerilog();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Lê as configurações do appsettings.json
    .Enrich.FromLogContext() // Adiciona informações de contexto
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

app.UseAuthentication();

app.UseAuthorization();

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
