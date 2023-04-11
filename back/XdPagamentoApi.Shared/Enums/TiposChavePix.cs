using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace XdPagamentoApi.Shared.Enums
{
    public enum TiposChavePix
    {
        [Description("Celular")]
        CE,
        [Description("Email")]
        EM,
        [Description("CPF")]
        CP,
        [Description("CNPJ")]
        CN,
        [Description("Chave Aleatorica")]
        CA
    }
}
