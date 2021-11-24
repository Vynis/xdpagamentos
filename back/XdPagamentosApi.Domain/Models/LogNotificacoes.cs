using System;
using System.Collections.Generic;
using System.Text;

namespace XdPagamentosApi.Domain.Models
{
    public class LogNotificacoes
    {
        public int Id { get; set; }
        public string NotificationCode { get; set; }
        public string NotificationType { get; set; }
        public DateTime Data { get; set; }

        public string Xml { get; set; }
        public string PublicKey { get; set; }
        public string NumTerminal { get; set; }
        public string EstId { get; set; }
        public string MotivoErro { get; set; }

    }
}
