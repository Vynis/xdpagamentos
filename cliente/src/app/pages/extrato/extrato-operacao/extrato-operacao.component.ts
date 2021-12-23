import { Router } from '@angular/router';
import { GestaoPagamentoService } from './../../../@core/services/gestao-pagamento-service';
import { GestaoPagamentoModel } from './../../../@core/models/gestao-pagamento.model';
import { ClienteModel } from './../../../@core/models/cliente.model';
import { ClienteService } from './../../../@core/services/cliente.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { ToastService } from '../../../@core/services/toast.service';
import { ToastPadrao } from '../../../@core/enums/toast.enum';

@Component({
  selector: 'ngx-extrato-operacao',
  templateUrl: './extrato-operacao.component.html',
  styleUrls: ['./extrato-operacao.component.scss']
})
export class ExtratoOperacaoComponent implements OnInit {
  existeErro: boolean = false;
  formulario: FormGroup;
  cliente: ClienteModel;

  constructor(
    private fb: FormBuilder, 
    private toastService : ToastService, 
    private route: Router,
    private clienteService: ClienteService, 
    private gestaoPagtoService: GestaoPagamentoService) { }

  ngOnInit(): void {
    this.buscarDadosCliente();
    this.createForm();
  }

  createForm() {
    this.formulario = this.fb.group({
      valorSolicitadoCliente: [null, Validators.required],
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

    if (!this.ehNumeric(controls.valorSolicitadoCliente.value)){
      this.existeErro = true;
      return false;
    }

    return true;
  }

  submit() {

    if (this.validacao() === false)
      return;

    const _model = new GestaoPagamentoModel();

    _model.valorSolicitadoCliente = this.formulario.controls.valorSolicitadoCliente.value;

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
