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
using XdPagamentosApi.Shared.Extensions;
using XdPagamentosApi.WebApi.Configuracao.Swagger;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TerminalController : BaseController
    {
        private readonly ITerminalService _terminalService;
        private readonly IMapper _mapper;

        public TerminalController(ITerminalService terminalService, IMapper mapper)
        {
            _terminalService = terminalService;
            _mapper = mapper;
        }

        [HttpPost("buscar-terminal-filtro")]
        [SwaggerGroup("Terminal")]
        public async Task<IActionResult> BuscarFiltro(PaginationFilter filtro)
        {
            try
            {
                return Response(_mapper.Map<DtoTerminalLista[]>(await _terminalService.BuscarComFiltro(filtro)));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpGet("buscar-por-id/{id}")]
        [SwaggerGroup("Terminal")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                return Response(await _terminalService.ObterPorId(id));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpPost("inserir")]
        [SwaggerGroup("Terminal")]
        public async Task<IActionResult> Inserir(DtoTerminal dto)
        {
            try
            {
                var validaNumTerminal =  await _terminalService.BuscarExpressao(x => x.NumTerminal.Equals(dto.NumTerminal));

                if (validaNumTerminal.Any())
                    return Response("Numero terminal já cadastrado", false);

                var response = await _terminalService.Adicionar(_mapper.Map<Terminal>(dto));

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
        [SwaggerGroup("Terminal")]
        public async Task<IActionResult> Alterar(DtoTerminal dto)
        {
            try
            {
                var dados = await _terminalService.ObterPorId(dto.Id);

                if (!dados.NumTerminal.Equals(dto.NumTerminal))
                {
                    var validaNumTerminal = await _terminalService.BuscarExpressao(x => x.NumTerminal.Equals(dto.NumTerminal));

                    if (validaNumTerminal.Any())
                        return Response("Numero terminal já cadastrado", false);

                }


                var response = await _terminalService.Atualizar(_mapper.Map<Terminal>(dto));

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
        [SwaggerGroup("Terminal")]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {

                var response = await _terminalService.ExcluirComValidacao(id);

                if (response.Count() > 0)
                    return Response(response, false);

                return Response("Exclusão com sucesso!");
            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpGet("buca-terminais-cliente/{id}")]
        [SwaggerGroup("Terminal")]
        public async Task<IActionResult> BuscaTerminaisCliente(int id)
        {
            try
            {
                return Response(await _terminalService.BuscaTerminalCliente(id));
            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpGet("buca-terminais-cliente")]
        [SwaggerGroup("Terminal")]
        public async Task<IActionResult> BuscaTerminaisCliente()
        {
            try
            {
                return Response(await _terminalService.BuscaTerminalCliente(Convert.ToInt32(User.Identity.Name.ToString().Descriptar())));
            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }
    }
}
