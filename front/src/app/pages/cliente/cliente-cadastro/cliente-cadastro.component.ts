import { ClienteModel } from './../../../@core/models/cliente.model';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { EstadosBrasileiros } from '../../../@core/enums/estados-brasileiros.enum';

@Component({
  selector: 'ngx-cliente-cadastro',
  templateUrl: './cliente-cadastro.component.html',
  styleUrls: ['./cliente-cadastro.component.scss']
})
export class ClienteCadastroComponent implements OnInit {

  tituloPagina: string = 'Cadastro de Clientes';
  existeErro: boolean = false;
  formulario: FormGroup;
  clienteOld: ClienteModel;
  listaestadosBrasileiros: any[];
  

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.listaestadosBrasileiros = EstadosBrasileiros;
    this.createForm(new ClienteModel());
  }

  createForm(_cliente: ClienteModel) {
    this.clienteOld = Object.assign({},_cliente);

    this.formulario = this.fb.group({
      id: [_cliente.id],
      tipoPessoa: [_cliente.tipoPessoa, Validators.required],
      estId: [_cliente.estId, Validators.required],
      nome: [_cliente.nome, Validators.required],
      cnpjCpf: [_cliente.cnpjCpf],
      endereco: [_cliente.endereco, Validators.required],
      cep: [_cliente.cep, Validators.required],
      bairro: [_cliente.bairro, Validators.required],
      cidade: [_cliente.cidade, Validators.required],
      estado: [_cliente.estado, Validators.required],
      fone1: [_cliente.fone1, Validators.required],
      fone2: [_cliente.fone2, Validators.required],
      email: [_cliente.email, [Validators.email]],
      status: [_cliente.status, Validators.required],
      possuiDadosBancario: [false, Validators.required],
      bandId: [_cliente.banId],
      numAgencia: [_cliente.numAgencia],
      numConta: [_cliente.numConta],
      tipoConta: [_cliente.tipoConta]
    });
  }

  submit(){}

  voltar(){}

}
