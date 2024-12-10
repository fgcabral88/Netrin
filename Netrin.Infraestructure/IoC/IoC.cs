using Microsoft.Extensions.DependencyInjection;
using Netrin.Application.Mappings;
using Netrin.Application.Services;
using Netrin.Domain.Service.Interfaces.Respository;
using Netrin.Domain.Service.Interfaces.Services;
using Netrin.Infraestructure.Repositories;

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

            #endregion

            return services;
        }
    }
}
