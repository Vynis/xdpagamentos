import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastPadrao } from '../../../@core/enums/toast.enum';
import { ClienteModel } from '../../../@core/models/cliente.model';
import { FormaPagtoModel } from '../../../@core/models/forma-pagto.model';
import { GestaoPagamentoModel } from '../../../@core/models/gestao-pagamento.model';
import { RelContaEstabelecimentoModel } from '../../../@core/models/rel-conta-estabelecimento.model';
import { ContaCaixaService } from '../../../@core/services/conta-caixa.service';
import { FormaPagtoService } from '../../../@core/services/forma-pagto.service';
import { GestaoExtratoService } from '../../../@core/services/gestao-extrato-service';
import { ToastService } from '../../../@core/services/toast.service';
import { formatarNumero } from '../../../@core/utils/funcoes';

@Component({
  selector: 'ngx-gestao-extrato-cadastro',
  templateUrl: './gestao-extrato-cadastro.component.html',
  styleUrls: ['./gestao-extrato-cadastro.component.scss']
})
export class GestaoExtratoCadastroComponent implements OnInit {
  tituloPagina: string = 'Cadastro de Gestao de Extrato';
  existeErro: boolean = false;
  formulario: FormGroup;
  listaClientes: ClienteModel[] = [];
  listaFormaPagto: FormaPagtoModel[] = [];
  listaContaCaixa: RelContaEstabelecimentoModel[] = [];
  gestaoPagtoOld: GestaoPagamentoModel;

  constructor(    
    private formaPagtoService: FormaPagtoService,
    private contaCaixaService: ContaCaixaService,
    private gestaoExtratoService: GestaoExtratoService,
    private fb: FormBuilder,
    private route : Router,
    private toastService : ToastService,) { }

  ngOnInit() {
    this.buscarFormaPagto();
    this.buscarContaCaixa();
    const model = new GestaoPagamentoModel();
    this.createForm(model);
  }

  createForm(_model: GestaoPagamentoModel) {
    this.gestaoPagtoOld = Object.assign({},_model);

    this.formulario = this.fb.group({
      id: [_model.id, Validators.required],
      dtHrLancamento: [_model.dtHrLancamento, Validators.required],
      descricao: [_model.descricao, Validators.required],
      tipo: [_model.tipo, Validators.required],
      vlBruto: [_model.vlBruto, Validators.required],
      fopId: [_model.fopId, Validators.required],
      rceId : [_model.rceId, Validators.required],
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

  buscarContaCaixa() {
    this.contaCaixaService.buscarContaCaixaEstabelecimento().subscribe(
      res => {
        if (!res.success)
          return;
        
        this.listaContaCaixa = res.data;
      }
    )
  }

  submit(): void {
    this.existeErro = false;

    if (this.validacao() === false)
      return;

    let conteudoModelPreparado = this.prepararModel();

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

    // if (!this.ehNumeric(controls.vlBruto.value)){
    //   this.existeErro = true;
    //   return false;
    // }
    
    return true;
  }

  voltar(): void {
    this.route.navigateByUrl('/pages/gestao-extrato');
  }

  prepararModel() : GestaoPagamentoModel { 
    const controls = this.formulario.controls;
    const _model = new GestaoPagamentoModel();

    _model.id = controls.id.value == null ? 0 : controls.id.value;
    _model.dtHrLancamento = controls.dtHrLancamento.value;
    _model.descricao = controls.descricao.value;
    _model.tipo = controls.tipo.value;
    _model.vlBruto = formatarNumero(controls.vlBruto.value);
    _model.fopId = controls.fopId.value;
    _model.rceId = controls.rceId.value;

    return _model;
  }

  inserir(_model: GestaoPagamentoModel) {
    this.gestaoExtratoService.inserir(_model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Cadastro realizado com sucesso!');
        this.route.navigateByUrl('/pages/gestao-extrato');
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
