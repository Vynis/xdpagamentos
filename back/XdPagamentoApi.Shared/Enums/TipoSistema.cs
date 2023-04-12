using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XdPagamentoApi.Shared.Enums
{
    public enum TipoSistema
    {
        [Description("Administrativa")]
        Admin,
        [Description("Cliente")]
        Cliente
    }
}
