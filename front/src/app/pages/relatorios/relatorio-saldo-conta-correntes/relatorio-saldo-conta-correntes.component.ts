import { VwRelatorioSaldoContaCorrente } from './../../../@core/models/vw-relatorio-saldo-conta-corrente.model';
import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { NbDialogService } from '@nebular/theme';
import { LocalDataSource } from 'ng2-smart-table';
import { FilterTypeConstants } from '../../../@core/enums/filter-type.enum';
import { ToastPadrao } from '../../../@core/enums/toast.enum';
import { FiltroItemModel } from '../../../@core/models/configuracao/filtroitem.model';
import { PaginationFilterModel } from '../../../@core/models/configuracao/paginationfilter.model';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { ContaCaixaModel } from '../../../@core/models/contacaixa.model';
import { ContaCaixaService } from '../../../@core/services/conta-caixa.service';
import { RelatoriosService } from '../../../@core/services/relatorios.service';
import { ToastService } from '../../../@core/services/toast.service';
import * as XLSX from 'xlsx';
import { jsPDF }  from 'jspdf';
import { AuthServiceService } from '../../../@core/services/auth-service.service';
import { SessoesEnum } from '../../../@core/enums/sessoes.enum';

@Component({
  selector: 'ngx-relatorio-saldo-conta-correntes',
  templateUrl: './relatorio-saldo-conta-correntes.component.html',
  styleUrls: ['./relatorio-saldo-conta-correntes.component.scss']
})
export class RelatorioSaldoContaCorrentesComponent implements OnInit {
  existeErro: boolean = false;
  formularioFiltro: FormGroup;
  settings: SettingsTableModel = new SettingsTableModel();
  source: LocalDataSource = new LocalDataSource();
  realizouFiltro: boolean = false;
  loading = false;
  loadingModal = false;
  @ViewChild('content', {static: false}) el!: ElementRef;
  @ViewChild('dialog', {static: true}) dialog: TemplateRef<any>;
  listaContaCaixa: ContaCaixaModel[] = [];
  listaSaldoContaCorrente: VwRelatorioSaldoContaCorrente[] = [];
  saldoFinalTotal = '0,00';
  saidasTotal = '0,00';
  entradasTotal = '0,00';

  constructor(
    private contaCaixaSerivce: ContaCaixaService, 
    private relatoriosService: RelatoriosService,
    private authService: AuthServiceService,
    private fb: FormBuilder,
    private dialogService: NbDialogService,
    private toastService : ToastService
  ) { }

  ngOnInit() {
    this.authService.validaPermissaoTela(SessoesEnum.RELATORIO_SALDO_CONTA_CORRENTES);
    this.createFormFiltro();
    this.buscarContaCaixa();
  }

  createFormFiltro() {
    this.formularioFiltro = this.fb.group({
      contaCaixa: ['T'],
    })
  }

  buscarContaCaixa() {
    this.contaCaixaSerivce.buscarAtivos().subscribe(
      res => {
        if (!res.success)
          return;

        this.listaContaCaixa = res.data;
      }
    )
  }

  pesquisar() {
    const controls = this.formularioFiltro.controls;
    var filtro = new PaginationFilterModel();
    let listaItem: FiltroItemModel[] = [];
    this.existeErro = false;
    this.realizouFiltro = false;


    if (controls.contaCaixa.value !== 'T') { 
      var item  = new FiltroItemModel();
      item.property = 'Id';
      item.filterType =  FilterTypeConstants.EQUALS ;
      item.value = controls.contaCaixa.value;
      listaItem.push(item);
    }

    filtro.filtro = listaItem;
    this.buscarDados(filtro);

  }


  buscarDados(filtro: PaginationFilterModel) {
    this.loading = true;
    this.relatoriosService.buscaRelatorioSaldoContaCorrente(filtro).subscribe(res => {
      this.loading = false;
      if (!res.success)
        return;

      if (res.data.length == 0){
        this.toastService.showToast(ToastPadrao.WARNING,'Atenção','Nenhum registro encontrado.');
        return;
      }

        
      this.listaSaldoContaCorrente = res.data.lista;
      this.saidasTotal = Number(res.data.saidasTotal).toFixed(2);
      this.entradasTotal = Number(res.data.entradasTotal).toFixed(2);
      this.saldoFinalTotal = Number(res.data.saldoFinalTotal).toFixed(2);
      this.dialogService.open(this.dialog);
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
      XLSX.writeFile(workbook, `RELATORIO_SALDO_CC_${dataAtual.toLocaleDateString()}_${dataAtual.getHours()}${dataAtual.getMinutes()}${dataAtual.getSeconds()}.xlsx`);
      this.loadingModal = false;
    }, 3000);
  }

  gerarPdf() {
    this.loadingModal = true;
    let pdf = new jsPDF('portrait','pt','A4');
    pdf.html(this.el.nativeElement, {
      margin: [5,5,5,5],
      callback: (pdf) => {
        this.loadingModal = false;
        var dataAtual = new Date();
        pdf.save(`RELATORIO_SALDO_CC_${dataAtual.toLocaleDateString()}_${dataAtual.getHours()}${dataAtual.getMinutes()}${dataAtual.getSeconds()}.pdf`);
      }
    })
  }


}
