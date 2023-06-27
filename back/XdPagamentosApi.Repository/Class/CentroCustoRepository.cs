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
    public class CentroCustoRepository : Base<CentroCusto>, ICentroCustoRepository
    {
        private readonly MySqlContext _mySqlContext;

        public CentroCustoRepository(MySqlContext mySqlContext) : base(mySqlContext)
        {
            _mySqlContext = mySqlContext;
        }

        public async Task<string[]> ExcluirComValidacao(int id)
        {
            var listaErros = new List<string>();

            var centroCusto = (await _mySqlContext.CentroCustos.Where(c => c.Id == id).ToArrayAsync()).FirstOrDefault();

            if (centroCusto == null)
                listaErros.Add("Centro Custro não encontrado");

            var relContaPagar = await _mySqlContext.ContaPagars.Where(c => c.CecId == id).ToArrayAsync();

            foreach (var item in relContaPagar)
                listaErros.Add($"Contas a Pagar (Id: {item.Id} | Descricao: {item.Descricao})");

            var relContaReber = await _mySqlContext.ContaRecers.Where(c => c.CecId == id).ToArrayAsync();

            foreach (var item in relContaReber)
                listaErros.Add($"Contas a Receber (Id: {item.Id} | Descricao: {item.Descricao})");

            if (listaErros.Count() == 0)
                await base.Excluir(centroCusto);
            
            return listaErros.ToArray();
        }
    }
}
