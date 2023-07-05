import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastService } from '../../../@core/services/toast.service';
import { ToastPadrao } from '../../../@core/enums/toast.enum';
import { formatarNumero, formatarNumeroUS } from '../../../@core/utils/funcoes';
import { ContaPagarService } from '../../../@core/services/conta-pagar.service';
import { ContaPagarModel } from '../../../@core/models/conta-pagar.model';
import { FluxoCaixaModel } from '../../../@core/models/fluxo-caixa.model';
import { ContaCaixaService } from '../../../@core/services/conta-caixa.service';
import { ContaCaixaModel } from '../../../@core/models/contacaixa.model';
import { PlanoContaModel } from '../../../@core/models/plano-conta.model';
import { PlanoContaService } from '../../../@core/services/plano-conta.service';
import { FluxoCaixaService } from '../../../@core/services/fluxo-caixa.service';

@Component({
  selector: 'app-conta-pagar-baixa',
  templateUrl: './conta-pagar-baixa.component.html',
  styleUrls: ['./conta-pagar-baixa.component.css']
})
export class ContaPagarBaixaComponent implements OnInit {
  formularioBaixa: FormGroup;
  contaPagar: ContaPagarModel;
  listaContaCaixa: ContaCaixaModel[];
  listaPlanoConta: PlanoContaModel[];
  listaPlanoContaDesconto: PlanoContaModel[];
  listaPlanoContaGeneric: PlanoContaModel[];
  itensFluxoCaixa: FluxoCaixaModel[];
  valorConta = 0;
  valorFluxo = 0;
  valorResultado = 0;
  valorContaTxt = '';
  valorFluxoTxt = '';
  valorResultadoTxt = '';
  existeErro = false;
  efetuouCalculo = false;
  calculoNormal = false;
  calculoAcrescimo = false;
  calculoDesconto = false;
  tituloCardAcDc = '';
  messageAlert = 'Favor preencher todos os campos obrigatorios!';

  constructor(
    private fb: FormBuilder,
    private route : Router,
    private activatedRoute: ActivatedRoute,
    private toastService : ToastService,
    private contaPagarService : ContaPagarService,
    private contaCaixaService: ContaCaixaService,
    private planoContaService: PlanoContaService,
    private fluxoCaixaService: FluxoCaixaService
  ) { }

  ngOnInit() {
    this.createFormBaixa();
    this.buscarContaCaixa();
    this.buscarPlanoConta();
    this.buscarPlanoContaDesconto();
    this.activatedRoute.params.subscribe(params => {
      const id = params.id;
      if (id && id > 0) {
        this.buscarDadosContasPagar(id);
      }
      else {
        this.voltar()
      }
    });
  }

  voltar() {
    this.route.navigateByUrl('/pages/conta-pagar');
  }

  buscarDadosContasPagar(id: number) {
    this.contaPagarService.buscaPorId(id).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
          console.log(res.data);
          return;
        }

        this.contaPagar = res.data;

        if (this.contaPagar.valor == "" || this.contaPagar.valor == ' 0,00'){
          this.toastService.showToast(ToastPadrao.DANGER, 'Não foi possível realizar a baixa sem informar o valor.');
          this.voltar();
        }
        
        this.buscarItensFluxoCaixa(id);
        
      },
      error => {
        console.log(error);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
      }
    )
  }

  buscarContaCaixa() {
    this.contaCaixaService.buscarAtivos().subscribe(
      res => {
        if (res.success) {
          this.listaContaCaixa = res.data;
        }
      }
    )
  }

  buscarPlanoConta() {
   this.planoContaService.buscarAtivos('S').subscribe(
    res => {
      if (res.success) {
        this.listaPlanoConta = res.data;
      }
    }
   ) 
  }

  buscarPlanoContaDesconto() {
    this.planoContaService.buscarAtivos('E').subscribe(
     res => {
       if (res.success) {
         this.listaPlanoContaDesconto = res.data;
       }
     }
    ) 
   }

  buscarItensFluxoCaixa(id) {
    this.fluxoCaixaService.buscarContas('CP', id).subscribe(
      res => {
        if (res.success) {
          this.itensFluxoCaixa = res.data;
          this.caculaSubTotal();
        }
      }
    )
  }

  
  createFormBaixa() {
    this.formularioBaixa = this.fb.group({
      id: [''],
      dtLancamento: [new Date(), Validators.required],
      descricao: [''],
      valor: [''],
      pcoId: ['', Validators.required],
      tipoBaixa: ['N'],
      cocId: ['', Validators.required],
      valorAcDc: [''],
      pcoIdAcDc: ['']

    })
  }

  validacao() : boolean {
    this.existeErro = false;
    this.messageAlert = 'Favor preencher todos os campos obrigatorios!';
    const controls = this.formularioBaixa.controls;

    if (this.formularioBaixa.invalid){
      Object.keys(controls).forEach(controlName => 
        controls[controlName].markAllAsTouched()
      );

      this.existeErro = true;
      return false;
    }


    if (controls.tipoBaixa.value == 'P' && (Number(controls.valor.value) > Number(this.valorResultado) || Number(controls.valor.value) == Number(this.valorResultado) )) {
      this.messageAlert = 'O valor parcial é maior/igual que valor total.';
      this.existeErro = true;
      return false;
    }

    return true;
  }

  validacaoAcDc() : boolean {
    this.existeErro = false;
    const controls = this.formularioBaixa.controls;

    if (controls.pcoIdAcDc.value == ''){
      this.messageAlert = 'Preencha o plano de conta.'
      return false;
    }
      
    return true;
  }

  submit() {
    this.existeErro = false;

    if (this.validacao() === false)
      return;

    if (this.calculoAcrescimo || this.calculoDesconto)
      if (this.validacaoAcDc() === false)
        return;

    let conteudoModelPreparado = this.prepararModel();

    this.inserir(conteudoModelPreparado);
  }

  submitAcDc() {
    this.existeErro = false;

    if (this.validacaoAcDc() === false)
    return;

    let conteudoModelPreparado = this.prepararModelAcDc();

    this.inserir(conteudoModelPreparado, true);
  }

  prepararModel() : FluxoCaixaModel { 
    const controls = this.formularioBaixa.controls;
    const _model = new FluxoCaixaModel();

    _model.descricao = controls.descricao.value;
    _model.valor = controls.tipoBaixa.value == 'N' ? formatarNumero( (-1) * Number(this.valorResultado) ) : formatarNumero( (-1) * Number(controls.valor.value) );
    _model.pcoId = controls.pcoId.value;
    _model.cocId = controls.cocId.value;
    _model.dtLancamento = controls.dtLancamento.value;
    _model.cpaId = this.contaPagar.id;
    _model.tipoPagamento = 'D';

    return _model;
  }

  prepararModelAcDc() : FluxoCaixaModel { 
    const controls = this.formularioBaixa.controls;
    const _model = new FluxoCaixaModel();

    _model.descricao = this.calculoAcrescimo ? 'Acréscimo' : 'Desconto';
    _model.valor = this.calculoAcrescimo ? formatarNumero((-1) * Number(controls.valorAcDc.value)) : formatarNumero(controls.valorAcDc.value);
    _model.pcoId = controls.pcoIdAcDc.value;
    _model.cocId = controls.cocId.value;
    _model.dtLancamento = controls.dtLancamento.value;
    _model.cpaId = this.contaPagar.id;
    _model.tipoPagamento = this.calculoAcrescimo ? 'D' : 'C';

    return _model;
  }

  inserir(model : FluxoCaixaModel, inserioAcDc: boolean = false) {
    this.fluxoCaixaService.inserir(model, this.formularioBaixa.controls.tipoBaixa.value).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Cadastro realizado com sucesso!');
        if ((this.calculoAcrescimo || this.calculoDesconto) && !inserioAcDc ) {
          this.submitAcDc();
        } else {
          this.voltar();
        }
        
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro');
      }
    )
  }

  caculaSubTotal() {
    this.valorConta = formatarNumeroUS(this.contaPagar.valor);
    this.itensFluxoCaixa.forEach(x => this.valorFluxo+= Number(formatarNumeroUS(x.valor)));
    this.valorResultado = this.valorConta - this.valorFluxo;

    this.valorContaTxt = formatarNumero(this.valorConta);
    this.valorFluxoTxt = formatarNumero(this.valorFluxo);
    this.valorResultadoTxt = formatarNumero(this.valorResultado);
  }

  calcularBaixa() {
    this.existeErro = false;

    if (this.validacao() === false)
      return;

    this.efetuouCalculo = true;
    this.desabilitarControles();
    const controls = this.formularioBaixa.controls;

    if (Number(controls.valor.value) > Number(this.valorResultado) && controls.tipoBaixa.value == 'N') {
      this.tituloCardAcDc = "Acréscimo";
      this.calculoNormal = false;
      this.calculoAcrescimo = true;
      controls.valorAcDc.setValue(Number(controls.valor.value) - Number(this.valorResultado));
      this.listaPlanoContaGeneric = this.listaPlanoConta;
      controls.pcoIdAcDc.setValue(145);
    }

    if (Number(controls.valor.value) < Number(this.valorResultado) && controls.tipoBaixa.value == 'N') {
      this.tituloCardAcDc = "Desconto";
      this.calculoNormal = false;
      this.calculoDesconto = true;
      controls.valorAcDc.setValue(Number(this.valorResultado) - Number(controls.valor.value));
      this.listaPlanoContaGeneric = this.listaPlanoContaDesconto;
      controls.pcoIdAcDc.setValue(176);
    }

    this.calculoNormal = true;

  }

  desabilitarControles() {
    const controls = this.formularioBaixa.controls;

    controls.dtLancamento.disable();
    controls.descricao.disable();
    controls.valor.disable();
    controls.pcoId.disable();
    controls.tipoBaixa.disable();
    controls.cocId.disable();

  }

  habilitarControles() {
    const controls = this.formularioBaixa.controls;

    controls.dtLancamento.enable();
    controls.descricao.enable();
    controls.valor.enable();
    controls.pcoId.enable();
    controls.tipoBaixa.enable();
    controls.cocId.enable();

  }

  corrigirBaixa() {
    this.efetuouCalculo = false;
    this.formularioBaixa.enable();
    this.calculoAcrescimo = false;
    this.calculoDesconto = false;
    this.existeErro = false;
    this.habilitarControles();
  }

}
