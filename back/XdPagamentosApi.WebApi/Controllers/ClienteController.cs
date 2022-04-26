using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentoApi.Shared.Helpers;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Class;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.WebApi.Configuracao.Swagger;
using XdPagamentosApi.WebApi.Dtos;
using XdPagamentosApi.WebApi.Shared;
using XdPagamentosApi.WebApi.Shared.Extensions;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : BaseController
    {
        private readonly IClienteService _clienteService;
        private readonly ITipoTransacaoService _tipoTransacaoService;
        private readonly IMapper _mapper;

        public ClienteController(IClienteService clienteService, ITipoTransacaoService tipoTransacaoService, IMapper mapper)
        {
            _clienteService = clienteService;
            _tipoTransacaoService = tipoTransacaoService;
            _mapper = mapper;
        }

        [HttpGet("buscar-dados-cliente")]
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> BuscarUsuario()
        {
            try
            {
                var response = await _clienteService.BuscarExpressao(x => x.Id == Convert.ToInt32(User.Identity.Name.ToString().Descriptar()) && x.Status.Equals("A"));

                if (!response.Any())
                    return Response("Usuario não encontrado", false);

                return Ok(new { name = response.FirstOrDefault().Nome, picture = "" });

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpGet("buscar-por-ativos")]
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> BuscarPorAtivos()
        {
            try
            {
                var response = await _clienteService.BuscarExpressao(x => x.Status.Equals("A"));

                return Response(response.ToList().OrderBy(c => c.Nome));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpGet("bucar-grupo-clientes")]
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> BuscarGrupoClientes()
        {
            try
            {
                var response = await _clienteService.ObterTodos();

                var grupos = response.ToList().Where(c => !string.IsNullOrEmpty(c.NomeAgrupamento)).GroupBy(c => c.NomeAgrupamento).Select(c => new { NomeAgrupamento = c.Key });

                return Response(grupos.ToList().OrderBy(c => c.NomeAgrupamento));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }


        [HttpPost("buscar-cliente-filtro")]
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> BuscarFiltro(PaginationFilter filtro)
        {
            try
            {
                return Response(_mapper.Map<DtoClienteLista[]>(await _clienteService.BuscarComFiltro(filtro)));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }


        [HttpGet("buscar-por-id/{id}")]
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            try
            {
                return Response(await _clienteService.ObterPorId(id));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpGet("buscar-dados-cliente-logado")]
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> BuscarDadosClienteLogado()
        {
            try
            {
                return Response(await _clienteService.ObterPorId(Convert.ToInt32(User.Identity.Name.ToString().Descriptar())));
            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }

        [HttpPost("inserir")]
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> Inserir(DtoCliente dtoCliente)
        {
            try
            {

                if (!string.IsNullOrEmpty(dtoCliente.CnpjCpf))
                {
                    var validaCpfCnpjExistente = await _clienteService.BuscarExpressao(x => x.CnpjCpf.Equals(dtoCliente.CnpjCpf));

                    if (validaCpfCnpjExistente.Any())
                        return Response("Cpf/Cnpj já cadastrado", false);
                }

                dtoCliente.Senha = SenhaHashService.CalculateMD5Hash("cli102030");
                dtoCliente.LimiteCredito = HelperFuncoes.ValorMoedaBRString(dtoCliente.LimiteCredito);

                var response = await _clienteService.Adicionar(_mapper.Map<Cliente>(dtoCliente));

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
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> Alterar(DtoCliente dtoCliente)
        {
            try
            {
                var dados = await _clienteService.ObterPorId(dtoCliente.Id);

                if (!string.IsNullOrEmpty(dtoCliente.CnpjCpf))
                    if (dados.CnpjCpf != dtoCliente.CnpjCpf)
                    {
                        var validaCpfCnpjExistente = await _clienteService.BuscarExpressao(x => x.CnpjCpf.Equals(dtoCliente.CnpjCpf));

                        if (validaCpfCnpjExistente.Any())
                            return Response("Cpf/Cnpj já cadastrado", false);

                    }


                dtoCliente.Senha = dados.Senha;
                dtoCliente.NomeAgrupamento = dados.NomeAgrupamento;
                dtoCliente.LimiteCredito = HelperFuncoes.ValorMoedaBRString(dtoCliente.LimiteCredito);


                var response = await _clienteService.Atualizar(_mapper.Map<Cliente>(dtoCliente));

                if (!response)
                    return Response("Erro ao alterar", false);

                return Response("Alteração com sucesso!");

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }


        [HttpPut("agrupar")]
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> Agrupar(List<DtoCliente> dtoCliente)
        {
            try
            {

                var listaClienteFormatada = new List<Cliente>();

                foreach( var item in dtoCliente)
                {
                    var cliente = await _clienteService.ObterPorId(item.Id);
                    cliente.NomeAgrupamento = item.NomeAgrupamento;
                    listaClienteFormatada.Add(cliente);
                }

                var response = await _clienteService.AtualizarLista(listaClienteFormatada);

                if (!response)
                    return Response("Erro ao alterar", false);

                return Response("Alteração com sucesso!");

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpPut("desagrupar")]
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> Desagrupar(List<DtoCliente> dtoCliente)
        {
            try
            {
                var listaClienteFormatada = new List<Cliente>();

                foreach (var item in dtoCliente)
                {
                    var cliente = await _clienteService.ObterPorId(item.Id);
                    cliente.NomeAgrupamento = "";
                    listaClienteFormatada.Add(cliente);
                }

                var response = await _clienteService.AtualizarLista(listaClienteFormatada);

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
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> Deletar(int id)
        {
            try
            {

                var response = await _clienteService.ExcluirComValidacao(id);

                if (response.Count() > 0)
                    return Response(response, false);

                return Response("Exclusão com sucesso!");
            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }


        [HttpPut("alterar-senha-padrao/{id}")]
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> AlterarSenha(int id)
        {
            try
            {
                var dados = await _clienteService.ObterPorId(id);

                if (dados == null)
                    return Response("Cliente não localizado!", false);

                dados.Senha = SenhaHashService.CalculateMD5Hash("cli102030");

                var response = await _clienteService.Atualizar(dados);

                if (!response)
                    return Response("Erro ao alterar", false);

                return Response("Alteração com sucesso!");

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

        [HttpPut("alterar-senha")]
        [SwaggerGroup("Cliente")]
        public async Task<IActionResult> AlterarSenha(DtoAlteracaoSenhaCliente dtoCliente)
        {
            try
            {

                var codClienteLogado = User.Identity.Name.ToString();

                if (codClienteLogado != dtoCliente.IdCliente)
                    return Response("Erro cliente nao encontrado", false);

                var idClienteFormatado = Convert.ToInt32(dtoCliente.IdCliente.ToString().Descriptar());

                var cliente = await _clienteService.BuscarExpressao(x => x.Id.Equals(idClienteFormatado) && x.Senha.Equals(SenhaHashService.CalculateMD5Hash(dtoCliente.SenhaAtual)));

                if (!cliente.Any())
                    return Response("Senha atual invalida", false);

                var usuarioEncontrado = cliente.FirstOrDefault();

                usuarioEncontrado.Senha = SenhaHashService.CalculateMD5Hash(dtoCliente.SenhaNova);

                var response = await _clienteService.Atualizar(usuarioEncontrado);

                if (!response)
                    return Response("Erro ao alterar", false);

                return Response("Alteração com sucesso!");

            }
            catch (Exception ex)
            {
                return Response(ex.Message, false);
            }
        }

    }
}
