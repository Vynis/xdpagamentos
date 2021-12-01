using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class EstabelecimentoMap : IEntityTypeConfiguration<Estabelecimento>
    {
        public void Configure(EntityTypeBuilder<Estabelecimento> builder)
        {
            builder.ToTable("tb_estabelecimentos");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("est_id");
            builder.Property(c => c.NumEstabelecimento).HasColumnName("est_num_estabelecimento");
            builder.Property(c => c.CnpjCpf).HasColumnName("est_cnpj_cpf");
            builder.Property(c => c.Nome).HasColumnName("est_nome");
            builder.Property(c => c.Endereco).HasColumnName("est_endereco");
            builder.Property(c => c.Bairro).HasColumnName("est_bairro");
            builder.Property(c => c.Cidade).HasColumnName("est_cidade");
            builder.Property(c => c.Estado).HasColumnName("est_estado");
            builder.Property(c => c.Cep).HasColumnName("est_cep");
            builder.Property(c => c.SaldoInicial).HasColumnName("est_saldo_inicial");
            builder.Property(c => c.NumBanco).HasColumnName("est_num_banco");
            builder.Property(c => c.NumAgencia).HasColumnName("est_num_agencia");
            builder.Property(c => c.NumConta).HasColumnName("est_num_conta");
            builder.Property(c => c.Status).HasColumnName("est_status");
            builder.Property(c => c.Tipo).HasColumnName("est_tipo");
            builder.Property(c => c.Token).HasColumnName("est_token");
            builder.Property(c => c.Email).HasColumnName("est_email");

            builder.Property(c => c.OpeId).HasColumnName("est_ope_id");


            builder.HasOne(c => c.Operadora).WithMany(c => c.ListaEstabelecimento).HasForeignKey(c => c.OpeId);

        }
    }
}
