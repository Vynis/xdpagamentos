import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NbDialogService } from '@nebular/theme';
import { LocalDataSource } from 'ng2-smart-table';
import { FilterTypeConstants } from '../../../@core/enums/filter-type.enum';
import { ToastPadrao } from '../../../@core/enums/toast.enum';
import { ClienteModel } from '../../../@core/models/cliente.model';
import { FiltroItemModel } from '../../../@core/models/configuracao/filtroitem.model';
import { PaginationFilterModel } from '../../../@core/models/configuracao/paginationfilter.model';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { EstabelecimentoModel } from '../../../@core/models/estabelecimento.model';
import { RelContaEstabelecimentoModel } from '../../../@core/models/rel-conta-estabelecimento.model';
import { RelatorioGeralVendasPorClienteModel } from '../../../@core/models/relatorio-geral-vendas-por-clientes.model';
import { ClienteService } from '../../../@core/services/cliente.service';
import { EstabelecimentoService } from '../../../@core/services/estabelecimento.service';
import { GestaoPagamentoService } from '../../../@core/services/gestao-pagamento-service';
import { ToastService } from '../../../@core/services/toast.service';
import * as XLSX from 'xlsx';
import { jsPDF }  from 'jspdf';

@Component({
  selector: 'ngx-relatorio-geral-vendas',
  templateUrl: './relatorio-geral-vendas.component.html',
  styleUrls: ['./relatorio-geral-vendas.component.scss']
})
export class RelatorioGeralVendasComponent implements OnInit {
  existeErro: boolean = false;
  formularioFiltro: FormGroup;
  settings: SettingsTableModel = new SettingsTableModel();
  source: LocalDataSource = new LocalDataSource();
  realizouFiltro: boolean = false;
  loading = false;
  loadingModal = false;
  listaEstabelecimentos: EstabelecimentoModel[] = [];
  listaClientes: ClienteModel[] = [];
  listaRelatorioGeralVendas: RelatorioGeralVendasPorClienteModel[] = [];
  @ViewChild('content', {static: false}) el!: ElementRef;
  @ViewChild('dialog', {static: true}) dialog: TemplateRef<any>;
  

  constructor(
    private estabelecimentoService: EstabelecimentoService
    ,private toastService: ToastService
    ,private clienteService: ClienteService
    ,private fb: FormBuilder
    ,private gestaoPagamentoService: GestaoPagamentoService
    ,private dialogService: NbDialogService ) { }

  ngOnInit() {
    this.buscarListaEstabelecimentos();
    this.buscaClientes();
    this.createFormFiltro();
  }

  buscarListaEstabelecimentos() {
    this.estabelecimentoService.buscarAtivos().subscribe(
      res => {
        
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
          console.log(res.data);
          return;
        }

        this.listaEstabelecimentos = res.data;

      },
      error => {
        console.log(error);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
      }
      
    )
  }

  buscaClientes() {
    this.clienteService.buscarAtivos().subscribe(
      res => {      
        if (!res.success)
          return;

        this.listaClientes = res.data;
      }
    )   
  }

  ehData(valor) {
    return (valor instanceof Date)
  }

  createFormFiltro() {
    this.formularioFiltro = this.fb.group({
      dtInicial: [new Date()],
      dtFinal: [new Date()],
      cliId: ['T'],
      estId: ['T']
    })
  }

  pesquisar() {
    const controls = this.formularioFiltro.controls;
    var filtro = new PaginationFilterModel();
    let listaItem: FiltroItemModel[] = [];
    this.existeErro = false;
    this.realizouFiltro = false;

    if ((controls.dtInicial.value === '' || !this.ehData(controls.dtInicial.value)) || (controls.dtFinal.value === '' || !this.ehData(controls.dtInicial.value))) {
      this.existeErro = true;
      return;
    }

    var item  = new FiltroItemModel();
    item.property = 'DtHrLancamento';
    item.filterType = FilterTypeConstants.GREATERTHANEQUALS;
    item.value = new Date(controls.dtInicial.value).toLocaleDateString();
    listaItem.push(item);

    var item  = new FiltroItemModel();
    item.property = 'DtHrLancamento';
    item.filterType = FilterTypeConstants.LESSTHANEQUALS;
    item.value =new Date(controls.dtFinal.value).toLocaleDateString() + ' 23:59';
    listaItem.push(item);


    if (controls.cliId.value !== 'T') { 
      var item  = new FiltroItemModel();
      item.property =  'CliId';
      item.filterType = FilterTypeConstants.EQUALS;
      item.value = controls.cliId.value;
      listaItem.push(item);
    }

    if (controls.estId.value !== 'T') { 
      var item  = new FiltroItemModel();
      item.property =  'EstId';
      item.filterType = FilterTypeConstants.EQUALS;
      item.value = controls.estId.value;
      listaItem.push(item);
    }

    filtro.filtro = listaItem;
    this.buscarDados(filtro);
  }

  buscarDados(filtro: PaginationFilterModel) {
    this.loading = true;
    this.gestaoPagamentoService.buscarRelatorioGestaoPagamento(filtro).subscribe(res => {
      this.loading = false;
      if (!res.success)
        return;

      if (res.data.length == 0){
        this.toastService.showToast(ToastPadrao.WARNING,'Atenção','Nenhum registro encontrado.');
        return;
      }

      res.data.forEach(el => {
        el.vlBrutoTotal = Number(el.vlBrutoTotal.replace('.','').replace(',','.')).toFixed(2);
        el.vlTxPagSeguroTotal = Number(el.vlTxPagSeguroTotal.replace('.','').replace(',','.')).toFixed(2);
        el.vlTxClienteTotal = Number(el.vlTxClienteTotal.replace('.','').replace(',','.')).toFixed(2);
        el.vlLiqOpeTotal = Number(el.vlLiqOpeTotal.replace('.','').replace(',','.')).toFixed(2);
        el.vlPagtoTotal = Number(el.vlPagtoTotal.replace('.','').replace(',','.')).toFixed(2);
        el.vlLucroTotal = Number(el.vlLucroTotal.replace('.','').replace(',','.')).toFixed(2);

        el.listaGestaoPagamento.forEach(element => {          
          element.vlBrutoTransacao = Number(element.vlBrutoTransacao.replace('.','').replace(',','.')).toFixed(2);
          element.valorTaxaPagSeguro = Number(element.valorTaxaPagSeguro.replace('.','').replace(',','.')).toFixed(2);
          element.valorPercentualTaxaPagSeguro = Number(element.valorPercentualTaxaPagSeguro.replace('.','').replace(',','.')).toFixed(2);
          element.valorTaxaPagCliente = Number(element.valorTaxaPagCliente.replace('.','').replace(',','.')).toFixed(2);
          element.valorPercentualTaxaPagCliente = Number(element.valorPercentualTaxaPagCliente.replace('.','').replace(',','.')).toFixed(2);
          element.valorLiquidoOperadora = Number(element.valorLiquidoOperadora.replace('.','').replace(',','.')).toFixed(2);
          element.vlLiquidoCliente = Number(element.vlLiquidoCliente.replace('.','').replace(',','.')).toFixed(2);
          element.valorLucroFormatado = Number(element.valorLucroFormatado.replace('.','').replace(',','.')).toFixed(2);
        });
      });

      this.listaRelatorioGeralVendas = res.data;
      this.dialogService.open(this.dialog, { dialogClass: 'model-full' });
    })
  }

  gerarPdf() {
    this.loadingModal = true;
    let pdf = new jsPDF('landscape','pt','A3');
    pdf.html(this.el.nativeElement, {
      margin: [5,5,5,5],
      callback: (pdf) => {
        this.loadingModal = false;
        var dataAtual = new Date();
        pdf.save(`RELATORIO_GERAL_VENDAS_${dataAtual.toLocaleDateString()}_${dataAtual.getHours()}${dataAtual.getMinutes()}${dataAtual.getSeconds()}.pdf`);
      }
    })
  }

  gerarExcel() {
    this.loadingModal = true;

    setTimeout(() => {

      let element = document.getElementById('tabela');
      var workbook = XLSX.utils.table_to_book(element);
  
      // Process Data (add a new row)
      var ws = workbook.Sheets["Sheet1"];
      XLSX.utils.sheet_add_aoa(ws, [], {origin:-1});
  
      // Package and Release Data (`writeFile` tries to write and save an XLSB file)
      var dataAtual = new Date();
      XLSX.writeFile(workbook, `RELATORIO_GERAL_VENDAS_${dataAtual.toLocaleDateString()}_${dataAtual.getHours()}${dataAtual.getMinutes()}${dataAtual.getSeconds()}.xls`);
      this.loadingModal = false;
    }, 3000);
  }

}
