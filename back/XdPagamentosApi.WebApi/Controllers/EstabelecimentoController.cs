using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentoApi.Shared.Dtos;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.Shared;
using XdPagamentosApi.WebApi.Configuracao.Swagger;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EstabelecimentoController : BaseController
    {
        private readonly IEstabelecimentoService _estabelecimentoService;
        private readonly IMapper _mapper;

        public EstabelecimentoController(IEstabelecimentoService estabelecimentoService, IMapper mapper)
        {
            _estabelecimentoService = estabelecimentoService;
            _mapper = mapper;
        }

        [HttpGet("buscar-por-ativos")]
        [SwaggerGroup("Estabelecimento")]
        public async Task<IActionResult> BuscarPorAtivos()
        {
            try
            {
                var response = await _estabelecimentoService.BuscarExpressao(x => x.Status.Equals("A"));

                return Response(response.ToList().OrderBy(c => c.Nome));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpPost("buscar-por-filtro")]
        [SwaggerGroup("Estabelecimento")]
        public async Task<IActionResult> BuscarPorFiltro(PaginationFilter filtro)
        {
            try
            {
                return Response(_mapper.Map<DtoEstabelecimentoLista[]>(await _estabelecimentoService.BuscarComFiltro(filtro)));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }


        [HttpGet("buscar-por-id/{id}")]
        [SwaggerGroup("Estabelecimento")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                return Response(await _estabelecimentoService.ObterPorId(id));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }


        [HttpPost("inserir")]
        [SwaggerGroup("Estabelecimento")]
        public async Task<IActionResult> Inserir(DtoEstabelecimento dto)
        {
            try
            {
                var validaCpfCnpjExistente = await _estabelecimentoService.BuscarExpressao(x => x.CnpjCpf.Equals(dto.CnpjCpf));

                if (validaCpfCnpjExistente.Any())
                    return Response("Cpf/Cnpj já cadastrado", false);

                dto.ListaRelContaEstabelecimento.ForEach(x => x.CreditoAutomatico = "S");

                var response = await _estabelecimentoService.Adicionar(_mapper.Map<Estabelecimento>(dto));

                if (!response)
                    return Response("Erro ao cadastrar", false);

                return Response("Cadastro com sucesso!");

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }


        [HttpPut("alterar")]
        [SwaggerGroup("Estabelecimento")]
        public async Task<IActionResult> Alterar(DtoEstabelecimento dto)
        {
            try
            {
                var dados = await _estabelecimentoService.ObterPorId(dto.Id);

                if (!dados.CnpjCpf.Equals(dto.CnpjCpf))
                {
                    var validaCpfCnpjExistente = await _estabelecimentoService.BuscarExpressao(x => x.CnpjCpf.Equals(dto.CnpjCpf));

                    if (validaCpfCnpjExistente.Any())
                        return Response("Cpf/Cnpj já cadastrado", false);

                }

                dto.ListaRelContaEstabelecimento.ForEach(x => x.CreditoAutomatico = "S");

                var response = await _estabelecimentoService.Atualizar(_mapper.Map<Estabelecimento>(dto));

                if (!response)
                    return Response("Erro ao alterar", false);

                return Response("Alteração com sucesso!");

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpDelete("deletar/{id}")]
        [SwaggerGroup("Estabelecimento")]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {

                var response = await _estabelecimentoService.ExcluirComValidacao(id);

                if (response.Count() > 0)
                    return Response(response, false);

                return Response("Exclusão com sucesso!");
            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }
    }
}
