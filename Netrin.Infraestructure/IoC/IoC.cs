using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Netrin.Application.Behaviors.Validations;
using Netrin.Application.Mappings;
using Netrin.Application.Services;
using Netrin.Domain.Service.Interfaces.Respository;
using Netrin.Domain.Service.Interfaces.Services;
using Netrin.Infraestructure.Repositories;
using Serilog;

namespace Netrin.Infraestructure.IoC
{
    public static class IoC
    {
        public static IServiceCollection AdicionarDependencias(this IServiceCollection services)
        {
            // Registrar Serviços
            services.AddScoped<IPessoasService, PessoasService>();

            // Registrar Repositórios
            services.AddScoped<IPessoasRepository, PessoasRepository>();

            #region Registra outros serviços

            // AutoMapper
            services.AddAutoMapper(typeof(PessoasProfile));

            // FluentValidation
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<CriarPessoasDtoValidation>();

            #endregion

            return services;
        }
    }
}
