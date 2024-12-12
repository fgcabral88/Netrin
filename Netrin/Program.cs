using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Netrin.Api.Presentation.Filters;
using Netrin.Application.Helpers;
using Netrin.Infraestructure.Data.Context;
using Netrin.Infraestructure.IoC;
using Serilog;
using System.Text;
using static Netrin.Application.Helpers.JwtTokenHelper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(options =>
{
    // Registrando os filtros
    options.Filters.Add<AcaoFilter>();
});
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

// Registrando JWT
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

// Registrando Injeção de dependências IoC
builder.Services.AdicionarDependencias();

// Registrando adiciona a string de conexão ao contêiner de serviços
builder.Services.AddScoped<SqlDbContext>(sp =>
    new SqlDbContext(builder.Configuration.GetConnectionString("SqlServer")!));

// Registrando o Serilog
builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {Message:lj}{NewLine}{Exception}")
        .WriteTo.MongoDB(
            databaseUrl: context.Configuration.GetConnectionString("MongoDB")!,
            collectionName: "Logs"
        )
        .Filter.ByExcluding(logEvent => logEvent.MessageTemplate.Text.Contains("Ação concluída")); 
});

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
