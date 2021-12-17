import { ContaCaixaModel } from './../../../@core/models/contacaixa.model';
import { ContaCaixaService } from './../../../@core/services/conta-caixa.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { LocalDataSource, Ng2SmartTableComponent } from 'ng2-smart-table';
import { SettingsTableModel } from '../../../@core/models/configuracao/table/settings.table.model';
import { ResumoLancamentosModel } from '../../../@core/models/resumo-lancamentos.model';
import { RelContaEstabelecimentoModel } from '../../../@core/models/rel-conta-estabelecimento.model';

@Component({
  selector: 'ngx-gestao-extrato-lista',
  templateUrl: './gestao-extrato-lista.component.html',
  styleUrls: ['./gestao-extrato-lista.component.scss']
})
export class GestaoExtratoListaComponent implements OnInit {
  existeErro: boolean = false;
  formularioFiltro: FormGroup;
  min: Date;
  max: Date;
  settings: SettingsTableModel = new SettingsTableModel();
  source: LocalDataSource = new LocalDataSource();
  realizouFiltro: boolean = false;
  resumoLancamentos: ResumoLancamentosModel = new ResumoLancamentosModel();
  @ViewChild('table') smartTable: Ng2SmartTableComponent;
  listaContaCaixa: ContaCaixaModel[] = [];
  lisRelContaEstabelecimento: RelContaEstabelecimentoModel[] = [];

  constructor(private contaCaixaSerivce: ContaCaixaService, private fb: FormBuilder) { }

  ngOnInit() {
    this.buscarContaCaixa();
    this.createFormFiltro();
  }

  createFormFiltro() {
    this.formularioFiltro = this.fb.group({
      contaCaixa: [0 ,Validators.required],
      estabelecimento: [0 ,Validators.required],
      descricao: [''],
      nomeCliente: [''],
      dtInicial: [new Date(), Validators.required],
      dtFinal: [new Date(), Validators.required],
      valorliquido: [''],
      tipo: ['T']
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

  buscaEstabelecimento(id: number) {
    this.contaCaixaSerivce.buscarContaCaixaEstabelecimentoPorId(id).subscribe(
      res => {
        if (!res.success)
          return;
          
        this.lisRelContaEstabelecimento = res.data;
      }
    )
  }

  itemSelecionado(evento) {
    this.formularioFiltro.controls.estabelecimento.setValue(0);
    this.buscaEstabelecimento(evento);
  }

  pesquisar() {}

  onCustom(event) {}

}
