using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("tb_usuarios");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("usu_id");
            builder.Property(c => c.Nome).HasColumnName("usu_nome");
            builder.Property(c => c.CPF).HasColumnName("usu_cpf");
            builder.Property(c => c.Senha).HasColumnName("usu_senha");
            builder.Property(c => c.Email).HasColumnName("usu_email");
            builder.Property(c => c.Status).HasColumnName("usu_status");
        }
    }
}
