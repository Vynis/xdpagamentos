import { RelContaEstabelecimentoModel } from './../../../@core/models/rel-conta-estabelecimento.model';
import { EstabelecimentoModel } from './../../../@core/models/estabelecimento.model';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ContaCaixaModel } from '../../../@core/models/contacaixa.model';
import { Router, ActivatedRoute } from '@angular/router';
import { EstabelecimentoService } from '../../../@core/services/estabelecimento.service';
import { ContaCaixaService } from '../../../@core/services/conta-caixa.service';
import { ToastService } from '../../../@core/services/toast.service';
import { ToastPadrao } from '../../../@core/enums/toast.enum';

@Component({
  selector: 'ngx-conta-caixa-cadastro',
  templateUrl: './conta-caixa-cadastro.component.html',
  styleUrls: ['./conta-caixa-cadastro.component.scss']
})
export class ContaCaixaCadastroComponent implements OnInit {

  tituloPagina: string = 'Cadastro de Conta Caixa';
  existeErro: boolean = false;
  formulario: FormGroup;
  contaOld: ContaCaixaModel;
  listaEstabelecimentos: EstabelecimentoModel[];
  listaEstablecimentoConta: EstabelecimentoModel[] = [];

  constructor(
    private fb: FormBuilder,
    private route : Router,
    private activatedRoute: ActivatedRoute,
    private toastService : ToastService,
    private estabelecimentoService: EstabelecimentoService,
    private contaCaixaService: ContaCaixaService
  ) { }

  ngOnInit(): void {
    this.buscarListaEstabelecimentos();

    this.activatedRoute.params.subscribe(params => {
      const id = params.id;
      if (id && id > 0) {
        this.tituloPagina = `Editar Conta Caixa - Nº ${id}`;
        this.buscaPorId(id);
      }
      else {
        const model = new ContaCaixaModel();
        this.createForm(model);
      }
    });
  }

  createForm(_conta: ContaCaixaModel) { 
    this.contaOld = Object.assign({},_conta);

    this.formulario = this.fb.group({
      id:  [_conta.id],
      descricao: [_conta.descricao, Validators.required],
      status: [_conta.status, Validators.required],
      estabelecimento: ['']
    });

    if (_conta.listaRelContaEstabelecimento.length > 0) {
      _conta.listaRelContaEstabelecimento.forEach(x => {
        this.listaEstablecimentoConta.push(x.estabelecimento);
      });
    }

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

  buscaPorId(id: number) {
    this.contaCaixaService.buscaPorId(id).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
          console.log(res.data);
          return;
        }

        this.createForm(res.data);
      },
      error => {
        console.log(error);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
      }
    )
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

    return true;
  }

  prepararModel() : ContaCaixaModel { 
    const controls = this.formulario.controls;
    const _model = new ContaCaixaModel();

    _model.id = controls.id.value == null ? 0 : controls.id.value;
    _model.descricao = controls.descricao.value;
    _model.status = controls.status.value;

    this.listaEstablecimentoConta.forEach(x => {
      _model.listaRelContaEstabelecimento.push({id: 0, cocId: 0, estId: x.id, contaCaixa: null, estabelecimento: null});
    });

    return _model;
  }

  inserir(model : ContaCaixaModel) {
    this.contaCaixaService.inserir(model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Cadastro realizado com sucesso!');
        this.route.navigateByUrl('/pages/conta');
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro');
      }
    )
  }

  alterar(model: ContaCaixaModel) {
    this.contaCaixaService.alterar(model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar alteração', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Alteração realizado com sucesso!');
        this.route.navigateByUrl('/pages/conta');
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o alteração');
      }
    )
  }
 
  submit() {
    this.existeErro = false;

    if (this.validacao() === false)
    return;

    let conteudoModelPreparado = this.prepararModel();

    if (conteudoModelPreparado.id > 0) {
      this.alterar(conteudoModelPreparado);
      return;
    }

    this.inserir(conteudoModelPreparado);
  }

  voltar() {
    this.route.navigateByUrl('/pages/conta');
  }

  addEstabelecimento() {
    const estabelecimentoCtrl = this.formulario.controls.estabelecimento.value;
    const valida = this.listaEstablecimentoConta.find(x => x.id == estabelecimentoCtrl.id);
    if (valida == null || valida == undefined )
      this.listaEstablecimentoConta.push(estabelecimentoCtrl);
  }

  excluirEstabelecimento(index) {
    this.listaEstablecimentoConta.splice(index,1);
  }

}
