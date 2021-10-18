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

    }
}
