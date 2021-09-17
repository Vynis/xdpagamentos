﻿using Microsoft.EntityFrameworkCore;
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

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Banco> Bancos { get; set; }
        public DbSet<Estabelecimento> Estabelecimentos { get; set; }
        public DbSet<Terminal> Terminais { get; set; }
        public DbSet<RelClienteTerminal> RelClienteTerminais { get; set; }
        public DbSet<ContaCaixa> ContaCaixas { get; set; }
        public DbSet<RelContaEstabelecimento> RelContaEstabelecimentos { get; set; }
        public DbSet<Operadora> Operadoras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Retira o delete on cascade
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                        .SelectMany(t => t.GetForeignKeys())
                        .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new ClienteMap());
            modelBuilder.ApplyConfiguration(new BancoMap());
            modelBuilder.ApplyConfiguration(new EstabelecimentoMap());
            modelBuilder.ApplyConfiguration(new TerminalMap());
            modelBuilder.ApplyConfiguration(new RelClienteTerminalMap());
            modelBuilder.ApplyConfiguration(new ContaCaixaMap());
            modelBuilder.ApplyConfiguration(new RelContaEstabelecimentoMap());
            modelBuilder.ApplyConfiguration(new OperadoraMap());

        }
    }
}
