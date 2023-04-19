using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentoApi.Shared.Dtos;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Domain.Models.NotificacaoTransacao;
using XdPagamentosApi.PagSeguro;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.Shared;
using XdPagamentosApi.WebApi.Configuracao.Swagger;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LogNotificacoesController : BaseController
    {
        private readonly ILogNotificacoesService _logNotificacoesService;
        private readonly IEstabelecimentoService _estabelecimentoService;
        private readonly IMapper _mapper;

        public LogNotificacoesController(ILogNotificacoesService logNotificacoesService, IEstabelecimentoService estabelecimentoService, IMapper mapper)
        {
            _logNotificacoesService = logNotificacoesService;
            _estabelecimentoService = estabelecimentoService;
            _mapper = mapper;
        }

        [HttpPost("notificacoes/{estabelecimento}")]
        [AllowAnonymous]
        [SwaggerGroup("LogNotificacoes")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Notificacoes([FromForm] DtoNotificacao notificacaoDto, string estabelecimento = "0")
        {
            try
            {

                var buscarEstabelecimento = await _estabelecimentoService.ObterPorId(Convert.ToInt32(estabelecimento));

                if (buscarEstabelecimento == null)
                    return  Response("Não foi possível fazer operacao", false);

                var urlConsultaNotificacao = "https://ws.pagseguro.uol.com.br/v3/transactions/notifications/";

                var apiPagSeguro = new PagSeguroAPI();

                var retornoPagSeguro = _mapper.Map<DtoTransactionPagSeguro>(apiPagSeguro.ConsultaPorCodigoNotificacao2(buscarEstabelecimento.Email, buscarEstabelecimento.Token, urlConsultaNotificacao, notificacaoDto.NotificationCode));

                var notificacoes = new LogNotificacoes();

                notificacoes.Data = DateTime.Now;
                notificacoes.NotificationCode = notificacaoDto.NotificationCode;
                notificacoes.NotificationType = notificacaoDto.NotificationType;
                notificacoes.Xml = retornoPagSeguro.Xml;
                notificacoes.EstId = estabelecimento;
                notificacoes.NumTerminal = retornoPagSeguro.DeviceInfo?.SerialNumber;
                notificacoes.PublicKey = retornoPagSeguro.PrimaryReceiver?.PublicKey;

                //Salva a notificacao
                var response = await _logNotificacoesService.Adicionar(notificacoes);

                if (!response)
                    return Response("Não foi possível fazer operacao", false);

                //if (!notificacoes.NumTerminal.Equals("1730545744"))
                //    return Ok();

                //Salva a ordem de pagamento
                var resonseOrdemPagto = await _logNotificacoesService.GerarOrdemPagamento(retornoPagSeguro, estabelecimento);

                if (!resonseOrdemPagto)
                    return Response("Não foi possível fazer operacao", false);

                return Ok();

            }
            catch (Exception ex)
            {
                await _logNotificacoesService.Adicionar(new LogNotificacoes { NotificationCode = notificacaoDto.NotificationCode, NotificationType = notificacaoDto.NotificationType, Xml = "erro", EstId = estabelecimento, Data = DateTime.Now, MotivoErro = ex.Message });
                return Response(ex.Message, false);
            }
        }
     }
}
