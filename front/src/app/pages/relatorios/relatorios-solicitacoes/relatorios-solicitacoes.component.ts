import { VwRelatorioSolicitacaoModel } from './../../../@core/models/vw-relatorio-solicitacao.model';
import { RelatoriosService } from './../../../@core/services/relatorios.service';
import { Component, OnInit, ViewChild, ElementRef, TemplateRef, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { LocalDataSource } from 'ng2-smart-table';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { FormaPagtoService } from '../../../@core/services/forma-pagto.service';
import { FormaPagtoModel } from '../../../@core/models/forma-pagto.model';
import { PaginationFilterModel } from '../../../@core/models/configuracao/paginationfilter.model';
import { FiltroItemModel } from '../../../@core/models/configuracao/filtroitem.model';
import { FilterTypeConstants } from '../../../@core/enums/filter-type.enum';
import { jsPDF }  from 'jspdf';
import { NbDialogService } from '@nebular/theme';
import { ToastService } from '../../../@core/services/toast.service';
import { ToastPadrao } from '../../../@core/enums/toast.enum';
import * as XLSX from 'xlsx';
import { AuthServiceService } from '../../../@core/services/auth-service.service';
import { SessoesEnum } from '../../../@core/enums/sessoes.enum';

@Component({
  selector: 'ngx-relatorios-solicitacoes',
  templateUrl: './relatorios-solicitacoes.component.html',
  styleUrls: ['./relatorios-solicitacoes.component.scss']
})
export class RelatoriosSolicitacoesComponent implements OnInit {
  existeErro: boolean = false;
  formularioFiltro: FormGroup;
  settings: SettingsTableModel = new SettingsTableModel();
  source: LocalDataSource = new LocalDataSource();
  realizouFiltro: boolean = false;
  listaFormaPagto: FormaPagtoModel[] = [];
  listaVwSolicitacoes: VwRelatorioSolicitacaoModel[] = [];
  @ViewChild('content', {static: false}) el!: ElementRef;
  @ViewChild('dialog', {static: true}) dialog: TemplateRef<any>;
  loading = false;
  loadingModal = false;

  constructor(
    private formaPagtoService: FormaPagtoService, 
    private relatoriosService: RelatoriosService,
    private authService: AuthServiceService,
    private fb: FormBuilder,
    private dialogService: NbDialogService,
    private toastService : ToastService) { }

  ngOnInit() {
    this.authService.validaPermissaoTela(SessoesEnum.RELATORIO_SOLICITACOES);
    this.buscarFormaPagto();
    this.createFormFiltro();
  }

  createFormFiltro() {
    this.formularioFiltro = this.fb.group({
      tipo: ['T'],
      valorTipo: [''],
      dtInicial: [new Date()],
      dtFinal: [new Date()],
      fopId: ['T'],
      status: ['T']
    })
  }

  buscarFormaPagto() {
    this.formaPagtoService.buscarAtivos().subscribe(
      res =>  {
        if (!res.success)
         return;

        this.listaFormaPagto = res.data;

      }
    )
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

    if (controls.tipo.value !== 'T') {
      if (controls.valorTipo.value === ''){
        this.existeErro = true;
        return;
      }
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

    if (controls.tipo.value !== 'T') { 
      var item  = new FiltroItemModel();
      item.property = controls.tipo.value == 'N' ? 'CliNome': 'CnpjCpf';
      item.filterType = controls.tipo.value == 'N' ?  FilterTypeConstants.CONTAINS : FilterTypeConstants.EQUALS ;
      item.value = controls.valorTipo.value;
      listaItem.push(item);
    }

    if (controls.fopId.value !== 'T') { 
      var item  = new FiltroItemModel();
      item.property =  'FopId';
      item.filterType = FilterTypeConstants.EQUALS;
      item.value = controls.fopId.value;
      listaItem.push(item);
    }


    if (controls.status.value !== 'T') { 
      var item  = new FiltroItemModel();
      item.property = 'StatusFormatado';
      item.filterType = FilterTypeConstants.EQUALS;
      item.value = controls.status.value;
      listaItem.push(item);
    }

    filtro.filtro = listaItem;
    this.buscarDados(filtro);

  }

  buscarDados(filtro: PaginationFilterModel) {
    this.loading = true;
    this.relatoriosService.buscaRelatorioSolicitacoes(filtro).subscribe(res => {
      this.loading = false;
      if (!res.success)
        return;

      if (res.data.length == 0){
        this.toastService.showToast(ToastPadrao.WARNING,'Atenção','Nenhum registro encontrado.');
        return;
      }

      res.data.forEach(element => {
        element.valorLiquido = Number(element.valorLiquido.replace('.','').replace(',','.')).toFixed(2);
      });
        
      this.listaVwSolicitacoes = res.data;
      this.dialogService.open(this.dialog, { dialogClass: 'model-full' });
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
      XLSX.writeFile(workbook, `RELATORIO_SOLICITACAO_${dataAtual.toLocaleDateString()}_${dataAtual.getHours()}${dataAtual.getMinutes()}${dataAtual.getSeconds()}.xls`);
      this.loadingModal = false;
    }, 3000);
  }

  ehData(valor) {
    return (valor instanceof Date)
  }

  gerarPdf() {
    this.loadingModal = true;
    let pdf = new jsPDF('landscape','pt','A4');
    pdf.html(this.el.nativeElement, {
      margin: [5,5,5,5],
      callback: (pdf) => {
        this.loadingModal = false;
        var dataAtual = new Date();
        pdf.save(`RELATORIO_SOLICITACAO_${dataAtual.toLocaleDateString()}_${dataAtual.getHours()}${dataAtual.getMinutes()}${dataAtual.getSeconds()}.pdf`);
      }
    })
  }

}
