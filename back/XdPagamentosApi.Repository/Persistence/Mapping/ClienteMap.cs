using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Enums;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class ClienteMap : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("tb_clientes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("cli_id");
            builder.Property(c => c.Nome).HasColumnName("cli_nome");
            builder.Property(c => c.Senha).HasColumnName("cli_senha");
            builder.Property(c => c.CnpjCpf).HasColumnName("cli_cnpj_cpf");
            builder.Property(c => c.Endereco).HasColumnName("cli_endereco");
            builder.Property(c => c.Bairro).HasColumnName("cli_bairro");
            builder.Property(c => c.Cidade).HasColumnName("cli_cidade");
            builder.Property(c => c.Estado).HasColumnName("cli_estado");
            builder.Property(c => c.Cep).HasColumnName("cli_cep");
            builder.Property(c => c.Fone1).HasColumnName("cli_fone1");
            builder.Property(c => c.Fone2).HasColumnName("cli_fone2");
            builder.Property(c => c.Email).HasColumnName("cli_email");
            builder.Property(c => c.NumAgencia).HasColumnName("cli_num_agencia");
            builder.Property(c => c.NumConta).HasColumnName("cli_num_conta");
            builder.Property(c => c.TipoConta).HasColumnName("cli_tipo_conta");
            builder.Property(c => c.Status).HasColumnName("cli_status");
            builder.Property(c => c.UltimoAcesso).HasColumnName("cli_ultimo_acesso");
            builder.Property(c => c.EstId).HasColumnName("cli_est_id");
            builder.Property(c => c.BanId).HasColumnName("cli_ban_id");
            builder.Property(c => c.TipoPessoa).HasColumnName("cli_tipo_pessoa");
            builder.Property(c => c.NomeAgrupamento).HasColumnName("cli_nome_agrupamento");
            builder.Property(c => c.LimiteCredito).HasColumnName("cli_limite_credito");
            builder.Property(c => c.TipoChavePix).HasColumnName("cli_tipo_chave_pix");
            builder.Property(c => c.ChavePix).HasColumnName("cli_chave_pix");

            builder.HasOne(c => c.Banco).WithMany().HasForeignKey(c => c.BanId);
            builder.HasOne(c => c.Estabelecimento).WithMany(c => c.ListaClientes).HasForeignKey(c => c.EstId);

            builder.Property(c => c.TipoChavePix).HasConversion( v => v.ToString(), v => (TiposChavePix)Enum.Parse(typeof(TiposChavePix),v) );


        }
    }
}
