import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CentroCustoModel } from '../../../@core/models/centro-custo.model';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthServiceService } from '../../../@core/services/auth-service.service';
import { CentroCustoService } from '../../../@core/services/centro-custo.service';
import { ToastService } from '../../../@core/services/toast.service';
import { SessoesEnum } from '../../../@core/enums/sessoes.enum';
import { ToastPadrao } from '../../../@core/enums/toast.enum';

@Component({
  selector: 'ngx-centro-custo-cadastro',
  templateUrl: './centro-custo-cadastro.component.html',
  styleUrls: ['./centro-custo-cadastro.component.scss']
})
export class CentroCustoCadastroComponent implements OnInit {

  tituloPagina: string = 'Cadastro de Centro Custo';
  existeErro: boolean = false;
  formulario: FormGroup;
  centroCustoOld: CentroCustoModel;

  constructor(
    private fb: FormBuilder,
    private route : Router,
    private activatedRoute: ActivatedRoute,
    private toastService : ToastService,
    private centroCustoService: CentroCustoService,
    private authService: AuthServiceService
  ) { }

  ngOnInit() {
    this.activatedRoute.params.subscribe(params => {
      const id = params.id;
      if (id && id > 0) {
        this.authService.validaPermissaoTela(SessoesEnum.ALTERAR_CENTRO_CUSTO);
        this.tituloPagina = `Editar Conta Caixa - Nº ${id}`;
        this.buscaPorId(id);
      }
      else {
        this.authService.validaPermissaoTela(SessoesEnum.CADASTRO_CENTRO_CUSTO);
        const model = new CentroCustoModel();
        this.createForm(model);
      }
    });
  }

  createForm(_centroCusto: CentroCustoModel) { 
    this.centroCustoOld = Object.assign({},_centroCusto);

    this.formulario = this.fb.group({
      id:  [_centroCusto.id],
      descricao: [_centroCusto.descricao, Validators.required],
      status: [_centroCusto.status, Validators.required]
    });
  }

  buscaPorId(id: number) {
    this.centroCustoService.buscaPorId(id).subscribe(
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

  prepararModel() : CentroCustoModel { 
    const controls = this.formulario.controls;
    const _model = new CentroCustoModel();

    _model.id = controls.id.value == null ? 0 : controls.id.value;
    _model.descricao = controls.descricao.value;
    _model.status = controls.status.value;

    return _model;
  }

  alterar(model: CentroCustoModel) {
    this.centroCustoService.alterar(model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar alteração', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Alteração realizado com sucesso!');
        this.route.navigateByUrl('/pages/centro-custo');
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o alteração');
      }
    )
  }

  inserir(model : CentroCustoModel) {
    this.centroCustoService.inserir(model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Cadastro realizado com sucesso!');
        this.route.navigateByUrl('/pages/centro-custo');
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro');
      }
    )
  }

  voltar() {
    this.route.navigateByUrl('/pages/centro-custo');
  }

}
