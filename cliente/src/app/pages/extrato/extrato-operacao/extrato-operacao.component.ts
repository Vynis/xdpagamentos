import { Router } from '@angular/router';
import { GestaoPagamentoService } from './../../../@core/services/gestao-pagamento-service';
import { GestaoPagamentoModel } from './../../../@core/models/gestao-pagamento.model';
import { ClienteModel } from './../../../@core/models/cliente.model';
import { ClienteService } from './../../../@core/services/cliente.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { ToastService } from '../../../@core/services/toast.service';
import { ToastPadrao } from '../../../@core/enums/toast.enum';
import { formatarNumero } from '../../../@core/utils/funcoes';
import { FormaPagtoModel } from '../../../@core/models/forma-pagto.model';
import { FormaPagtoService } from '../../../@core/services/forma-pagto.service';
import { CustomValidators } from '../../../@core/utils/custom-validator';

@Component({
  selector: 'ngx-extrato-operacao',
  templateUrl: './extrato-operacao.component.html',
  styleUrls: ['./extrato-operacao.component.scss']
})
export class ExtratoOperacaoComponent implements OnInit {
  existeErro: boolean = false;
  formulario: FormGroup;
  cliente: ClienteModel;
  listaFormaPagto: FormaPagtoModel[] = [];
  min: Date;


  constructor(
    private fb: FormBuilder, 
    private toastService : ToastService, 
    private route: Router,
    private clienteService: ClienteService, 
    private gestaoPagtoService: GestaoPagamentoService,
    private formaPagtoService: FormaPagtoService) {
     this.min = new Date();
    }

  ngOnInit(): void {
    this.buscarFormaPagto();
    this.buscarDadosCliente();
    this.createForm();
  }

  createForm() {

    const date = new Date();
    date.setDate(date.getDate() + 1);

    this.formulario = this.fb.group({
      valorSolicitadoCliente: [null, Validators.required],
      fopId: [0, Validators.required],
      dtAgendamento: [date, [Validators.required]]
    });
  }

  buscarDadosCliente() {
    this.clienteService.buscaDadosCliente().subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
          console.log(res.data);
          return;
        }
        
        this.cliente = res.data;
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

    // if (!this.ehNumeric(controls.valorSolicitadoCliente.value)){
    //   this.existeErro = true;
    //   return false;
    // }

    return true;
  }

  submit() {

    if (this.validacao() === false)
      return;

    const _model = new GestaoPagamentoModel();

    _model.valorSolicitadoCliente = formatarNumero(this.formulario.controls.valorSolicitadoCliente.value);
    _model.fopId = this.formulario.controls.fopId.value;
    _model.dtAgendamento = this.formulario.controls.dtAgendamento.value;

    this.gestaoPagtoService.solicitarPagto(_model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar solicitação', res.data);
          console.log(res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Cadastro realizado com sucesso!');
        this.route.navigateByUrl('/pages/extrato');
      }
    )


  }

  voltar() {
    this.route.navigateByUrl('/pages/extrato');
  }

  ehNumeric(value) {
    return /^\d+(?:\,\d+)?$/.test(value);
  }

}
