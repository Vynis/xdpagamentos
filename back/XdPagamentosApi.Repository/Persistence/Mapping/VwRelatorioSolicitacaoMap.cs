using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class VwRelatorioSolicitacaoMap : IEntityTypeConfiguration<VwRelatorioSolicitacao>
    {
        public void Configure(EntityTypeBuilder<VwRelatorioSolicitacao> builder)
        {
            builder.ToTable("VW_REL_SOLICITACOES");
        }
    }
}
