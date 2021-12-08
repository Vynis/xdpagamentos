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

  constructor(
    private clienteService: ClienteService,
    private formaPagtoService: FormaPagtoService,
    private contaCaixaService: ContaCaixaService,
    private fb: FormBuilder,
    private route : Router,
    private activatedRoute: ActivatedRoute,
    private toastService : ToastService,
  ) { }

  ngOnInit(): void {
    this.buscaClientes();
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
      vlLiquido: [_model.vlLiquido, Validators.required],
      cliId: [_model.cliId, Validators.required],
      fopId: [_model.fopId, Validators.required],
      rceId : [_model.rceId, Validators.required],
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

  submit(): void {
    this.existeErro = false;

    if (this.validacao() === false)
      return;

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

  voltar(): void {
    this.route.navigateByUrl('/pages/gestao-pagto');
  }

}
