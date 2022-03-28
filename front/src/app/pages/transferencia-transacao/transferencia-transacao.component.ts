import { ClienteModel } from './../../@core/models/cliente.model';
import { ClienteService } from './../../@core/services/cliente.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { NbDateService } from '@nebular/theme';
import { FilterTypeConstants } from '../../@core/enums/filter-type.enum';
import { ToastPadrao } from '../../@core/enums/toast.enum';
import { FiltroItemModel } from '../../@core/models/configuracao/filtroitem.model';
import { PaginationFilterModel } from '../../@core/models/configuracao/paginationfilter.model';
import { EstabelecimentoModel } from '../../@core/models/estabelecimento.model';
import { ParamOrdemPagtoModel } from '../../@core/models/param-ordem-pagto.model';
import { RelContaEstabelecimentoModel } from '../../@core/models/rel-conta-estabelecimento.model';
import { TransacoesSemOrdemPagtoPorClienteModel } from '../../@core/models/transacoes-sem-ordem-pagto-cliente.model';
import { TransacoesSemOrdemPagtoPorTerminalModel } from '../../@core/models/transacoes-sem-ordem-pagto-terminal.model';
import { ContaCaixaService } from '../../@core/services/conta-caixa.service';
import { EstabelecimentoService } from '../../@core/services/estabelecimento.service';
import { OrdemPagtoService } from '../../@core/services/ordem-pagto.service';
import { ToastService } from '../../@core/services/toast.service';
import { TransferenciaPagtoService } from '../../@core/services/transferencia-pago.service';
import { AuthServiceService } from '../../@core/services/auth-service.service';
import { SessoesEnum } from '../../@core/enums/sessoes.enum';

@Component({
  selector: 'app-transferencia-transacao',
  templateUrl: './transferencia-transacao.component.html',
  styleUrls: ['./transferencia-transacao.component.scss']
})
export class TransferenciaTransacaoComponent implements OnInit {

  listaEstabelecimento: EstabelecimentoModel[] = [];
  listaRelContaCaixaEstabeleicmento: RelContaEstabelecimentoModel[] = [];
  listaTransacoesSemOrdemPagto: TransacoesSemOrdemPagtoPorTerminalModel[] = [];
  listaTransacoesSemOrdemPagtoSelecionado: TransacoesSemOrdemPagtoPorTerminalModel[] = [];
  listaClientes: ClienteModel[] = [];
  formularioFiltro: FormGroup;
  formularioGeracao: FormGroup;
  min: Date;
  max: Date;
  realizouFiltro: boolean = false;
  existeErro:boolean = false;
  existeErro2:boolean = false;


  constructor(
    private estabelecimentoService: EstabelecimentoService,
    private contaCaixaService: ContaCaixaService, 
    private transferenciaPagtoService: TransferenciaPagtoService,
    private clienteService: ClienteService,
    private fb: FormBuilder,
    protected dateService: NbDateService<Date>,
    private toastService : ToastService,
    private authService: AuthServiceService
    ) { 
      // this.min = this.dateService.addMonth(this.dateService.today(), -1);
      // this.max = this.dateService.addMonth(this.dateService.today(), 1);
      this.validaPermissao();
    }

  ngOnInit(): void {
    this.buscarEstabelecimentos();
    this.buscarContaCaixaRelEstabelecimento();
    this.buscaClientes();

    this.createFormFiltro();
    this.createFormGeracao();

  }

  private validaPermissao() { 
    this.authService.validaPermissaoTela(SessoesEnum.LISTA_TRANSFERENCIA_TRANSACAO);
    this.authService.permissaoUsuario().subscribe(res => {
      if (!res.success)
        return;

      // const validaExclusao = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.EXCLUIR_ESTABELECIMENTO);
      // const validaAlteracao = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.ALTERAR_ESTABELECIMENTO);
      // this.validaNovo = this.authService.validaPermissaoAvulsa(res.data, SessoesEnum.CADASTRO_ESTABELECIMENTO);
      // this.carregaPagina = true;

      // this.buscaDados(new PaginationFilterModel);
      // this.configuracaoesGrid(validaExclusao,validaAlteracao );

    });
  }

  

  createFormFiltro() {
    this.formularioFiltro = this.fb.group({
      periodoGeral: [false],
      estabelecimento: [0, Validators.required],
      dtInicial: [new Date()],
      dtFinal: [new Date()],
      status: ['NP']
    })
  }

  createFormGeracao() {
    this.formularioGeracao = this.fb.group({
      conta: [null, Validators.required],
      cliente: [null, Validators.required],
      dtLancamento: [new Date(), Validators.required]
    })
  }

  buscarEstabelecimentos() {
    this.estabelecimentoService.buscarAtivos().subscribe(
      res => { 
        if (res.success) {
          this.listaEstabelecimento = res.data;
        }
      }
    )
  }

  buscarContaCaixaRelEstabelecimento() {
    this.contaCaixaService.buscarContaCaixaEstabelecimento().subscribe(
      res => {
        if (res.success) {
          this.listaRelContaCaixaEstabeleicmento = res.data;
        }
      }
    )
  }

  buscaDados(filtro: PaginationFilterModel) {
    this.transferenciaPagtoService.buscarTransacoesSemOrdemPagto(filtro).subscribe(
      res => {
        
        if (res.success) {
          this.listaTransacoesSemOrdemPagto = res.data;
          this.realizouFiltro = true;
        }
      },
      err => {
        alert(err)
      }
    )
  }

  buscaClientes() {
    this.clienteService.buscarAtivos().subscribe(
      res => {
        if (res.success) {
          this.listaClientes = res.data;
        }
      }
    )
  }


  tranformaNomeEstabelecimento(texto) {
    if (texto.length > 15)
      texto = texto.substring(0,12) + '...';

    return texto;
  }

  selecionaTerminal(evento: boolean, item : TransacoesSemOrdemPagtoPorTerminalModel) {
    if (evento) {
      this.listaTransacoesSemOrdemPagtoSelecionado.push(item);
    }
    else {
      this.listaTransacoesSemOrdemPagtoSelecionado = this.listaTransacoesSemOrdemPagtoSelecionado.filter(c => c !== item);
    }
  }

  pesquisar() {

    // if (this.validacaoFiltro() === false)
    //   return;

    const controls = this.formularioFiltro.controls;
    var filtro = new PaginationFilterModel();
    let listaItem: FiltroItemModel[] = [];
    

    if (controls.estabelecimento.value !== 0) {
      var item  = new FiltroItemModel();
      item.property = 'EstId';
      item.filterType = FilterTypeConstants.EQUALS;
      item.value = controls.estabelecimento.value;
      listaItem.push(item);
    }

    if (!controls.periodoGeral.value) {

      if ((controls.dtInicial.value !== '' && this.ehData(controls.dtInicial.value))  && (controls.dtFinal.value !== '' && this.ehData(controls.dtInicial.value)) ) {
        var item  = new FiltroItemModel();
        item.property = 'DataOperacao';
        item.filterType = FilterTypeConstants.GREATERTHANEQUALS;
        item.value = new Date(controls.dtInicial.value).toLocaleDateString();
        listaItem.push(item);
  
  
        var item  = new FiltroItemModel();
        item.property = 'DataOperacao';
        item.filterType = FilterTypeConstants.LESSTHANEQUALS;
        item.value =new Date(controls.dtFinal.value).toLocaleDateString() + ' 23:59';
        listaItem.push(item);
    
      }
    }

    // if (controls.status.value !== null) {
    //   var item  = new FiltroItemModel();
    //   item.property = 'Status';
    //   item.filterType = FilterTypeConstants.EQUALS;
    //   item.value = controls.status.value;
    //   listaItem.push(item);
    // }



    filtro.filtro = listaItem;
    this.buscaDados(filtro);

  }

  ehData(valor) {
    return (valor instanceof Date)
  }

  validacaoFiltro() : boolean {
    const controls = this.formularioFiltro.controls;
    this.existeErro = false;

    if (this.formularioFiltro.invalid){
      Object.keys(controls).forEach(controlName => 
        controls[controlName].markAllAsTouched()
      );

      this.existeErro = true;
      return false;
    }

    return true;
  }

  validacaoFormGeracao() : boolean {
    const controls = this.formularioGeracao.controls;
    this.existeErro2 = false;

    if (this.formularioGeracao.invalid){
      Object.keys(controls).forEach(controlName => 
        controls[controlName].markAllAsTouched()
      );

      this.existeErro2 = true;
      return false;
    }

    if (this.listaTransacoesSemOrdemPagtoSelecionado.length == 0) {
      this.existeErro2 = true;
      return false;
    }
      
    return true;
  }

  submit() {

    if (this.validacaoFormGeracao() === false)
       return;

    const controls = this.formularioGeracao.controls;

    var parametro = new ParamOrdemPagtoModel();

    parametro.idConta = controls.conta.value;
    parametro.idCliente = controls.cliente.value;
    parametro.dataLancamentoCredito = controls.dtLancamento.value;
    parametro.terminaisSelecionados = this.listaTransacoesSemOrdemPagtoSelecionado;

    this.transferenciaPagtoService.gerarOrdemPagto(parametro).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar a geracao', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Ordem de pagamento gerado com sucesso!');
        this.listaTransacoesSemOrdemPagtoSelecionado = [];
        this.pesquisar();
      }
    )

  }



}
