using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class RelUsuarioEstabelecimentoMap : IEntityTypeConfiguration<RelUsuarioEstabelecimento>
    {
        public void Configure(EntityTypeBuilder<RelUsuarioEstabelecimento> builder)
        {
            builder.ToTable("tb_rel_usuarios_estabelecimentos");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("rue_id");
            builder.Property(c => c.UsuId).HasColumnName("rue_usu_id");
            builder.Property(c => c.EstId).HasColumnName("rue_est_id");

            builder.HasOne(c => c.Usuario).WithMany(c => c.ListaUsuarioEstabelecimentos).HasForeignKey(c => c.UsuId);
            builder.HasOne(c => c.Estabelecimento).WithMany(c => c.ListaUsuarioEstabelecimentos).HasForeignKey(c => c.EstId);
        }
    }
}
