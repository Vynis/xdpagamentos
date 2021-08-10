using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.WebApi.Dtos;

namespace XdPagamentosApi.WebApi
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Usuario, DtoUsuarioLogado>().ReverseMap();
            CreateMap<Usuario, DtoUsuario>().ReverseMap();
            CreateMap<Cliente, DtoCliente>().ReverseMap();
            CreateMap<Estabelecimento, DtoEstabelecimento>().ReverseMap();
        }
    }
}
