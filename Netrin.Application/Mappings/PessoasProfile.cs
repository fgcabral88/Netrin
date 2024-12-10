using AutoMapper;
using Netrin.Application.Dtos.Pessoa;
using Netrin.Domain.Entities;

namespace Netrin.Application.Mappings
{
    public class PessoasProfile : Profile
    {
        public PessoasProfile()
        {
            // Entidade para Dto | Dto para Entidade
            CreateMap<PessoasEntity, CriarPessoasDto>().ReverseMap();
            CreateMap<PessoasEntity, EditarPessoasDto>().ReverseMap();
            CreateMap<PessoasEntity, ListarPessoasDto>().ReverseMap();
        }
    }
}
