using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using XdPagamentosApi.Domain.Models;

namespace XdPagamentosApi.Repository.Persistence.Mapping
{
    public class LogNotificacoesMap : IEntityTypeConfiguration<LogNotificacoes>
    {
        public void Configure(EntityTypeBuilder<LogNotificacoes> builder)
        {
            builder.ToTable("tb_log_notificacoes");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id).HasColumnName("lon_id");
            builder.Property(c => c.NotificationCode).HasColumnName("lon_notification_code");
            builder.Property(c => c.NotificationType).HasColumnName("lon_notification_type");
            builder.Property(c => c.Data).HasColumnName("lon_data");
            builder.Property(c => c.Xml).HasColumnName("lon_xml");
            builder.Property(c => c.PublicKey).HasColumnName("lon_public_key");
            builder.Property(c => c.NumTerminal).HasColumnName("lon_num_terminal");
            builder.Property(c => c.EstId).HasColumnName("lon_est_id");
            builder.Property(c => c.MotivoErro).HasColumnName("lon_motivo_erro");
        }
    }
}
