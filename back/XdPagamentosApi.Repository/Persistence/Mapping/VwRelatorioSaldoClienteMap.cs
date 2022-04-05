using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class VwRelatorioSaldoClienteMap : IEntityTypeConfiguration<VwRelatorioSaldoCliente>
    {
        public void Configure(EntityTypeBuilder<VwRelatorioSaldoCliente> builder)
        {
            builder.ToTable("VW_RELATORIO_SALDO_CLIENTES");
        }
    }
}
