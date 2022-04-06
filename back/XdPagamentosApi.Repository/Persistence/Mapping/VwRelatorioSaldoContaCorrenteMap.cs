using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class VwRelatorioSaldoContaCorrenteMap : IEntityTypeConfiguration<VwRelatorioSaldoContaCorrente>
    {
        public void Configure(EntityTypeBuilder<VwRelatorioSaldoContaCorrente> builder)
        {
            builder.ToTable("VW_RELATORIO_SALDO_CONTA_CORRENTE");
        }
    }
}
