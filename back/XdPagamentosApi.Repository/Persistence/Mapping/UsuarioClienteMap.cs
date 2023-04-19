using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class UsuarioClienteMap : IEntityTypeConfiguration<UsuarioCliente>
    {
        public void Configure(EntityTypeBuilder<UsuarioCliente> builder)
        {
            builder.ToTable("tb_usuario_cliente");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("usc_id");
            builder.Property(c => c.Nome).HasColumnName("usc_nome");
            builder.Property(c => c.Senha).HasColumnName("usc_senha");
            builder.Property(c => c.Email).HasColumnName("usc_email");
            builder.Property(c => c.Status).HasColumnName("usc_status");

        }
    }
}
