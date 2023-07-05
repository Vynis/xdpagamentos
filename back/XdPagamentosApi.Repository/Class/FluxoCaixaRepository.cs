using FiltrDinamico.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XdPagamentosApi.Domain.Models;
using XdPagamentosApi.Repository.Interfaces;
using XdPagamentosApi.Repository.Persistence.Context;

namespace XdPagamentosApi.Repository.Class
{
    public class FluxoCaixaRepository : Base<FluxoCaixa>, IFluxoCaixaRepository
    {
        private readonly MySqlContext _mySqlContext;
        private readonly IFiltroDinamico _filtroDinamico;

        public FluxoCaixaRepository(MySqlContext mySqlContext, IFiltroDinamico filtroDinamico) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
            _filtroDinamico = filtroDinamico;
        }

        public async Task<bool> AdicionarComBaixa(FluxoCaixa obj, string tipo)
        {
            var result = await base.Adicionar(obj);

            if (tipo.Equals("N"))
            {
               if (obj.CpaId != 0)
                {
                    var contaPagar = await  _mySqlContext.ContaPagars.Where(x => x.Id == obj.CpaId && x.Status.Equals("NP")).AsNoTracking().FirstOrDefaultAsync();
                    if (contaPagar != null)
                    {
                        contaPagar.Status = "PG";
                        await new ContaPagarRepository(_mySqlContext, _filtroDinamico).Atualizar(contaPagar);
                    }               
                }

               if (obj.CorId != 0)
                {
                    var contaReceber = await _mySqlContext.ContaRecers.Where(x => x.Id == obj.CorId && x.Status.Equals("NP")).AsNoTracking().FirstOrDefaultAsync();

                    if (contaReceber != null)
                    {
                        contaReceber.Status = "PG";
                        await new ContaReceberRepository(_mySqlContext, _filtroDinamico).Atualizar(contaReceber);
                    }
                }
            }

            return result;
        }

        public async Task<bool> Restaurar(int id, string conta)
        {
            if (conta.Equals("CP")) {
                var contaPagar = await _mySqlContext.ContaPagars.Where(x => x.Id == id).Include(c => c.ListaFluxoCaixa).AsNoTracking().FirstOrDefaultAsync();
                contaPagar.Status = "NP";
                await new ContaPagarRepository(_mySqlContext, _filtroDinamico).Atualizar(contaPagar);
                return await base.ExcluirLista(contaPagar.ListaFluxoCaixa.ToArray());
            }

            if (conta.Equals("CR"))
            {
                var contaReceber = await _mySqlContext.ContaRecers.Where(x => x.Id == id).Include(c => c.ListaFluxoCaixa).AsNoTracking().FirstOrDefaultAsync();
                contaReceber.Status = "NP";
                await new ContaReceberRepository(_mySqlContext, _filtroDinamico).Atualizar(contaReceber);
                return await base.ExcluirLista(contaReceber.ListaFluxoCaixa.ToArray());
            }

            return false;
        }
    }
}
