using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class FormaPagtoMap : IEntityTypeConfiguration<FormaPagto>
    {
        public void Configure(EntityTypeBuilder<FormaPagto> builder)
        {
            builder.ToTable("tb_forma_pagto");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("fop_id");
            builder.Property(c => c.Descricao).HasColumnName("fop_descricao");
            builder.Property(c => c.Status).HasColumnName("fop_status");
        }
    }
}

