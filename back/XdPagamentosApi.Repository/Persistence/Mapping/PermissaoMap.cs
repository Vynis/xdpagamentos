using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class PermissaoMap : IEntityTypeConfiguration<Permissao>
    {
        public void Configure(EntityTypeBuilder<Permissao> builder)
        {
            builder.ToTable("tb_permissoes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("per_id");
            builder.Property(c => c.SesId).HasColumnName("per_ses_id");
            builder.Property(c => c.UsuId).HasColumnName("per_usu_id");

            builder.HasOne(c => c.Usuario).WithMany(c => c.ListaPermissao).HasForeignKey(c => c.UsuId);
            builder.HasOne(c => c.Sessao).WithMany(c => c.ListaPermissao).HasForeignKey(c => c.SesId);
        }
    }
}
