import { OperadoraService } from './../../../@core/services/operadora.service';
import { OperadoraModel } from './../../../@core/models/operadora.model';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { EstadosBrasileiros } from '../../../@core/enums/estados-brasileiros.enum';
import { ToastPadrao } from '../../../@core/enums/toast.enum';
import { ContaCaixaModel } from '../../../@core/models/contacaixa.model';
import { EstabelecimentoModel } from '../../../@core/models/estabelecimento.model';
import { ContaCaixaService } from '../../../@core/services/conta-caixa.service';
import { EstabelecimentoService } from '../../../@core/services/estabelecimento.service';
import { ToastService } from '../../../@core/services/toast.service';
import { AuthServiceService } from '../../../@core/services/auth-service.service';
import { SessoesEnum } from '../../../@core/enums/sessoes.enum';

@Component({
  selector: 'ngx-estabelecimento-cadastro',
  templateUrl: './estabelecimento-cadastro.component.html',
  styleUrls: ['./estabelecimento-cadastro.component.scss']
})
export class EstabelecimentoCadastroComponent implements OnInit {

  tituloPagina: string = 'Cadastro de Estabelecimentos';
  existeErro: boolean = false;
  formulario: FormGroup;
  estabelecimentoOld: EstabelecimentoModel;
  listaestadosBrasileiros: any[];
  listaContaCaixa: ContaCaixaModel[];
  listaOperadoras: OperadoraModel[];

  constructor(
    private fb: FormBuilder,
    private route : Router,
    private activatedRoute: ActivatedRoute,
    private estabelecimentoService: EstabelecimentoService,
    private contaCaixaService: ContaCaixaService,
    private operadoraService: OperadoraService,
    private toastService : ToastService,
    private authService: AuthServiceService
  ) { }

  ngOnInit(): void {
    this.listaestadosBrasileiros = EstadosBrasileiros;
    this.buscaListaOperadoras();
    this.buscaListaContaCaixaService();

    this.activatedRoute.params.subscribe(params => {
      const id = params.id;
      if (id && id > 0) {
        this.authService.validaPermissaoTela(SessoesEnum.ALTERAR_ESTABELECIMENTO);
        this.tituloPagina = `Editar Estabelecimento - Nº ${id}`;
        this.buscaPorId(id);
      }
      else {
        this.authService.validaPermissaoTela(SessoesEnum.CADASTRO_ESTABELECIMENTO);
        const model = new EstabelecimentoModel();
        this.createForm(model);
      }
    });
  }

  createForm(_estabeblecimento: EstabelecimentoModel) {
    this.estabelecimentoOld = Object.assign({},_estabeblecimento);

    this.formulario = this.fb.group({
      id: [_estabeblecimento.id],
      nome: [_estabeblecimento.nome, Validators.required],
      cnpjCpf: [_estabeblecimento.cnpjCpf, Validators.required],
      endereco: [_estabeblecimento.endereco, Validators.required],
      cep: [_estabeblecimento.cep, Validators.required],
      bairro: [_estabeblecimento.bairro, Validators.required],
      cidade: [_estabeblecimento.cidade, Validators.required],
      estado: [_estabeblecimento.estado, Validators.required],
      numEstabelecimento: [_estabeblecimento.numEstabelecimento, Validators.required],
      status: [_estabeblecimento.status, Validators.required],
      opeId: [_estabeblecimento.opeId, Validators.required],
      cocId: [_estabeblecimento.cocId, Validators.required]
    });
  }

  buscaListaContaCaixaService() {
    this.contaCaixaService.buscarAtivos().subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
          console.log(res.data);
          return;
        }

        this.listaContaCaixa = res.data;
      },
      error => {
        console.log(error);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
      }
    )
  }

  buscaListaOperadoras() {
    this.operadoraService.buscarAtivos().subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
          console.log(res.data);
          return;
        }

        this.listaOperadoras = res.data;
      },
      error => {
        console.log(error);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
      }
    )
  }

  buscaPorId(id: number) {
    this.estabelecimentoService.buscaPorId(id).subscribe(
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

  submit(){
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

  voltar(){
    this.route.navigateByUrl('/pages/estabelecimento');
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


  prepararModel() : EstabelecimentoModel {
    const controls = this.formulario.controls;
    const _estabelecimento = new EstabelecimentoModel();

    _estabelecimento.id = controls.id.value == null ? 0 : controls.id.value;
    _estabelecimento.cnpjCpf = controls.cnpjCpf.value;
    _estabelecimento.nome = controls.nome.value;
    _estabelecimento.cep = controls.cep.value;
    _estabelecimento.endereco = controls.endereco.value;
    _estabelecimento.bairro = controls.bairro.value;
    _estabelecimento.cidade = controls.cidade.value;
    _estabelecimento.estado = controls.estado.value;
    _estabelecimento.status = controls.status.value;
    _estabelecimento.numEstabelecimento = controls.numEstabelecimento.value;
    _estabelecimento.cocId = controls.cocId.value;
    _estabelecimento.opeId = controls.opeId.value;

    return _estabelecimento;
  }


  inserir(model : EstabelecimentoModel) {
    this.estabelecimentoService.inserir(model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Cadastro realizado com sucesso!');
        this.route.navigateByUrl('/pages/estabelecimento');
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro');
      }
    )
  }

  alterar(model: EstabelecimentoModel) {
    this.estabelecimentoService.alterar(model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar alteração', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Alteração realizado com sucesso!');
        this.route.navigateByUrl('/pages/estabelecimento');
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o alteração');
      }
    )
  }

}
