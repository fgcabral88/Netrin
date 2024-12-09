using Microsoft.OpenApi.Models;

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"));
}

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
