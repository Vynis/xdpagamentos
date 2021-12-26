import { GestaoPagamentoService } from './../../../@core/services/gestao-pagamento-service';
import { GestaoPagamentoModel } from './../../../@core/models/gestao-pagamento.model';
import { ContaCaixaService } from './../../../@core/services/conta-caixa.service';
import { FormaPagtoService } from './../../../@core/services/forma-pagto.service';
import { ClienteService } from './../../../@core/services/cliente.service';
import { RelContaEstabelecimentoModel } from './../../../@core/models/rel-conta-estabelecimento.model';

import { FormaPagtoModel } from './../../../@core/models/forma-pagto.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { ClienteModel } from '../../../@core/models/cliente.model';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastService } from '../../../@core/services/toast.service';
import { isNumeric } from 'rxjs/internal-compatibility';
import { ContentObserver } from '@angular/cdk/observers';
import { ToastPadrao } from '../../../@core/enums/toast.enum';

@Component({
  selector: 'ngx-gestao-pagamento-cadastro',
  templateUrl: './gestao-pagamento-cadastro.component.html',
  styleUrls: ['./gestao-pagamento-cadastro.component.scss']
})
export class GestaoPagamentoCadastroComponent implements OnInit {
  tituloPagina: string = 'Cadastro de Gestao de Pagamento';
  existeErro: boolean = false;
  formulario: FormGroup;
  listaClientes: ClienteModel[] = [];
  listaFormaPagto: FormaPagtoModel[] = [];
  listaContaCaixa: RelContaEstabelecimentoModel[] = [];
  gestaoPagtoOld: GestaoPagamentoModel;
  ehAprovacao: boolean = false;

  constructor(
    private clienteService: ClienteService,
    private formaPagtoService: FormaPagtoService,
    private contaCaixaService: ContaCaixaService,
    private gestaoPagtoService: GestaoPagamentoService,
    private fb: FormBuilder,
    private route : Router,
    private activatedRoute: ActivatedRoute,
    private toastService : ToastService,
  ) { }

  ngOnInit(): void {
    this.buscaClientes();
    this.buscarFormaPagto();
    this.buscarContaCaixa();



    this.activatedRoute.params.subscribe(params => {
      const id = params.id;
      if (id && id > 0) {
        //this.authService.validaPermissaoTela(SessoesEnum.ALTERAR_USUARIOS);
        this.tituloPagina = `Aprovar solicitacão do cliente - Nº ${id}`;
        this.buscaPorId(id);
      }
      else {
        //this.authService.validaPermissaoTela(SessoesEnum.CADASTRO_USUARIOS);
        const model = new GestaoPagamentoModel();
        this.createForm(model);
      }
    });
  }

  createForm(_model: GestaoPagamentoModel) {
    this.gestaoPagtoOld = Object.assign({},_model);

    this.formulario = this.fb.group({
      id: [_model.id, Validators.required],
      dtHrLancamento: [new Date(_model.dtHrLancamento), Validators.required],
      descricao: [_model.descricao, Validators.required],
      tipo: [_model.tipo, Validators.required],
      vlLiquido: [this.ehAprovacao ? _model.valorSolicitadoCliente : _model.vlLiquido, Validators.required],
      cliId: [_model.cliId, Validators.required],
      fopId: [_model.fopId, Validators.required],
      rceId : [_model.rceId, Validators.required],
      status: [_model.status]
    })

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

  buscarFormaPagto() {
    this.formaPagtoService.buscarAtivos().subscribe(
      res =>  {
        if (!res.success)
         return;

        this.listaFormaPagto = res.data;

      }
    )
  }

  buscarContaCaixa() {
    this.contaCaixaService.buscarContaCaixaEstabelecimento().subscribe(
      res => {
        if (!res.success)
          return;
        
        this.listaContaCaixa = res.data;
      }
    )
  }

  buscaPorId(id) {
    this.gestaoPagtoService.buscaPorId(id).subscribe(
      res => {
        if (!res.success)
          return;

        this.ehAprovacao = true;
        this.createForm(res.data);
      }
    )
  }

  submit(): void {
    this.existeErro = false;

    if (this.validacao() === false)
      return;

    let conteudoModelPreparado = this.prepararModel();

    if (this.ehAprovacao)
      this.alterar(conteudoModelPreparado);
    else
      this.inserir(conteudoModelPreparado);

  }

  validacao() : boolean {
    this.existeErro = false;
    const controls = this.formulario.controls;

    if (this.formulario.invalid){
      Object.keys(controls).forEach(controlName => 
        controls[controlName].markAllAsTouched()
      );

      this.existeErro = true;
      return false;
    }

    if (!this.ehNumeric(controls.vlLiquido.value)){
      this.existeErro = true;
      return false;
    }

    if (this.ehAprovacao)
      if (controls.status.value === null || controls.status.value === '') {
        this.existeErro = true;
        return false;
      }
    
    return true;
  }

  voltar(): void {
    this.route.navigateByUrl('/pages/gestao-pagto');
  }

  prepararModel() : GestaoPagamentoModel { 
    const controls = this.formulario.controls;
    const _model = new GestaoPagamentoModel();

    _model.id = controls.id.value == null ? 0 : controls.id.value;
    _model.dtHrLancamento = controls.dtHrLancamento.value;
    _model.descricao = controls.descricao.value;
    _model.tipo = controls.tipo.value;
    _model.vlLiquido = controls.vlLiquido.value;
    _model.cliId = controls.cliId.value;
    _model.fopId = controls.fopId.value;
    _model.rceId = controls.rceId.value;
    
    if (this.ehAprovacao)
      _model.status = controls.status.value;

    return _model;
  }

  inserir(_model: GestaoPagamentoModel) {
    this.gestaoPagtoService.inserir(_model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Cadastro realizado com sucesso!');
        this.route.navigateByUrl('/pages/gestao-pagto');
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro');
      }
    )
  }

  alterar(_model: GestaoPagamentoModel) {
    this.gestaoPagtoService.alterar(_model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o alteracao', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Aprovação realizado com sucesso!');
        this.route.navigateByUrl('/pages/gestao-pagto');
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro');
      }
    )
  }

  ehNumeric(value) {
    return /^\d+(?:\,\d+)?$/.test(value);
  }

}
