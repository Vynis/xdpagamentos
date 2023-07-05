import { Component, OnInit } from '@angular/core';
import { ContaReceberModel } from '../../../@core/models/conta-receber.model';
import { CentroCustoModel } from '../../../@core/models/centro-custo.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastPadrao } from '../../../@core/enums/toast.enum';
import { CentroCustoService } from '../../../@core/services/centro-custo.service';
import { ContaReceberService } from '../../../@core/services/conta-receber.service';
import { ToastService } from '../../../@core/services/toast.service';
import { formatarNumero, formatarNumeroUS } from '../../../@core/utils/funcoes';

@Component({
  selector: 'ngx-conta-receber-cadastro',
  templateUrl: './conta-receber-cadastro.component.html',
  styleUrls: ['./conta-receber-cadastro.component.scss']
})
export class ContaReceberCadastroComponent implements OnInit {

  tituloPagina: string = 'Cadastro de Contas a Receber';
  existeErro: boolean = false;
  contasReceberOld: ContaReceberModel;
  listaCentroCusto: CentroCustoModel[];
  formulario: FormGroup;

  constructor(
    private fb: FormBuilder,
    private route : Router,
    private activatedRoute: ActivatedRoute,
    private toastService : ToastService,
    private centroCustoService: CentroCustoService,
    private contaReceberService: ContaReceberService
  ) { }

  ngOnInit() {
    this.buscaCentroCusto();

    this.activatedRoute.params.subscribe(params => {
      const id = params.id;
      if (id && id > 0) {
        //this.authService.validaPermissaoTela(SessoesEnum.ALTERAR_USUARIOS);
        this.tituloPagina = `Editar Conta Receber - Nº ${id}`;
        this.buscaPorId(id);
      }
      else {
        //this.authService.validaPermissaoTela(SessoesEnum.CADASTRO_USUARIOS);
        const model = new ContaReceberModel();
        model.dataEmissao = new Date();
        this.createForm(model);
      }
    });
  }

  voltar() {
    this.route.navigateByUrl('/pages/conta-receber');
  }

  createForm(_model: ContaReceberModel) {
    this.contasReceberOld = Object.assign({},_model);
   

    this.formulario = this.fb.group({
      id: [_model.id],
      descricao: [_model.descricao, Validators.required],
      valor: [_model.valor],
      valorPrevisto: [_model.valorPrevisto, Validators.required],
      dataEmissao: [new Date(_model.dataEmissao), Validators.required],
      dataVencimento: [new Date(_model.dataVencimento), Validators.required],
      centroCusto: [_model.cecId, Validators.required],
      obs: [_model.obs]
    });
  }

  buscaCentroCusto() {
    this.centroCustoService.buscarAtivos().subscribe(
      res => {
        if (!res.success)
          return;

        this.listaCentroCusto = res.data;
      }
    )
  }

  prepararModel() : ContaReceberModel { 
    const controls = this.formulario.controls;
    const _model = new ContaReceberModel();

    _model.id = controls.id.value == null ? 0 : controls.id.value;
    _model.descricao = controls.descricao.value;
    _model.valor = formatarNumero(controls.valor.value);
    _model.valorPrevisto = formatarNumero(controls.valorPrevisto.value);
    _model.dataVencimento = controls.dataVencimento.value;
    _model.dataEmissao = controls.dataEmissao.value;
    _model.obs = controls.obs.value;
    _model.cecId = controls.centroCusto.value;

    return _model;
  }

  inserir(model : ContaReceberModel) {
    this.contaReceberService.inserir(model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Cadastro realizado com sucesso!');
        this.route.navigateByUrl('/pages/conta-receber');
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro');
      }
    )
  }

  alterar(model: ContaReceberModel) {
    this.contaReceberService.alterar(model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar alteração', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Alteração realizado com sucesso!');
        this.route.navigateByUrl('/pages/conta-receber');
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

  buscaPorId(id: number) {
    this.contaReceberService.buscaPorId(id).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
          console.log(res.data);
          return;
        }

        res.data.valor = formatarNumeroUS(res.data.valor);
        res.data.valorPrevisto = formatarNumeroUS(res.data.valorPrevisto);

        this.createForm(res.data);
      },
      error => {
        console.log(error);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
      }
    )
  }
}
