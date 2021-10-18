using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Services.Interfaces;
using XdPagamentosApi.WebApi.Configuracao.Swagger;
using XdPagamentosApi.WebApi.Dtos;
using XdPagamentosApi.WebApi.Shared;

namespace XdPagamentosApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogNotificacoesController : BaseController
    {
        private readonly ILogNotificacoesService _logNotificacoesService;

        public LogNotificacoesController(ILogNotificacoesService logNotificacoesService)
        {
            _logNotificacoesService = logNotificacoesService;
        }

        [HttpPost("notificacoes")]
        [AllowAnonymous]
        [SwaggerGroup("LogNotificacoes")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> Notificacoes([FromForm] DtoNotificacao notificacaoDto)
        {
            try
            {
                //Inicia com log
                var logNotificacoes = new LogNotificacoes()
                {
                    Data = DateTime.Now,
                    NotificationCode = notificacaoDto.NotificationCode,
                    NotificationType = notificacaoDto.NotificationType
                };

                var response = await _logNotificacoesService.Adicionar(logNotificacoes);

                if (!response)
                    return Response("Não foi possível fazer operacao", false);

                return Ok();

            }
            catch (Exception ex)
            {

                return Response(ex.Message, false);
            }
        }
     }
}
