using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Persistence.Mapping;

namespace XdPagamentosApi.Repository.Persistence.Context
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options)
        {
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Banco> Bancos { get; set; }
        public DbSet<Estabelecimento> Estabelecimentos { get; set; }
        public DbSet<Terminal> Terminais { get; set; }
        public DbSet<RelClienteTerminal> RelClienteTerminais { get; set; }
        public DbSet<ContaCaixa> ContaCaixas { get; set; }
        public DbSet<RelContaEstabelecimento> RelContaEstabelecimentos { get; set; }
        public DbSet<Operadora> Operadoras { get; set; }
        public DbSet<Sessao> Sessoes { get; set; }
        public DbSet<Permissao> Permissoes { get; set; }
        public DbSet<RelUsuarioEstabelecimento> RelUsuarioEstabelecimentos { get; set; }
        public DbSet<TipoTransacao> TipoTransacoes { get; set; }
        public DbSet<LogNotificacoes> LogNotificacoes { get; set; }
        public DbSet<VwTransacoesSemOrdemPagto> VwTransacoesSemOrdemPagtos { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<Pagamentos> Pagamentos { get; set; }
        public DbSet<OrdemPagto> OrdemPagtos { get; set; }
        public DbSet<FormaPagto> FormaPagtos { get; set; }
        public DbSet<StatusTransacao> StatusTransacoes { get; set; }
        public  DbSet<TipoOperacao> TipoOperacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Retira o delete on cascade
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                        .SelectMany(t => t.GetForeignKeys())
                        .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

            ConfiguracaoMap(modelBuilder);

        }

        private static void ConfiguracaoMap(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new ClienteMap());
            modelBuilder.ApplyConfiguration(new BancoMap());
            modelBuilder.ApplyConfiguration(new EstabelecimentoMap());
            modelBuilder.ApplyConfiguration(new TerminalMap());
            modelBuilder.ApplyConfiguration(new RelClienteTerminalMap());
            modelBuilder.ApplyConfiguration(new ContaCaixaMap());
            modelBuilder.ApplyConfiguration(new RelContaEstabelecimentoMap());
            modelBuilder.ApplyConfiguration(new OperadoraMap());
            modelBuilder.ApplyConfiguration(new SessaoMap());
            modelBuilder.ApplyConfiguration(new PermissaoMap());
            modelBuilder.ApplyConfiguration(new RelUsuarioEstabelecimentoMap());
            modelBuilder.ApplyConfiguration(new TipoTransacaoMap());
            modelBuilder.ApplyConfiguration(new LogNotificacoesMap());
            modelBuilder.ApplyConfiguration(new VwTransacoesSemOrdemPagtoMap());
            modelBuilder.ApplyConfiguration(new PagamentosMap());
            modelBuilder.ApplyConfiguration(new OrdemPagtoMap());
            modelBuilder.ApplyConfiguration(new FormaPagtoMap());
            modelBuilder.ApplyConfiguration(new TransacaoMap());
            modelBuilder.ApplyConfiguration(new StatusTransacaoMap());
            modelBuilder.ApplyConfiguration(new GestaoPagamentoMap());
            modelBuilder.ApplyConfiguration(new TipoOperacaoMap());
        }
    }
}
