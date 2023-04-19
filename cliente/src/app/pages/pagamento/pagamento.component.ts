import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TiposChavePix } from '../../@core/enums/tipos-chave-pix';
import { ToastPadrao } from '../../@core/enums/toast.enum';
import { BancoModel } from '../../@core/models/banco.model';
import { ClienteModel } from '../../@core/models/cliente.model';
import { BancoService } from '../../@core/services/banco.service';
import { ClienteService } from '../../@core/services/cliente.service';
import { ToastService } from '../../@core/services/toast.service';
import { GeralEnum } from '../../@core/enums/geral.enum';

@Component({
  selector: 'ngx-pagamento',
  templateUrl: './pagamento.component.html',
  styleUrls: ['./pagamento.component.scss']
})
export class PagamentoComponent implements OnInit {
  formulario: FormGroup;
  clienteOld: ClienteModel;
  listaBancos: BancoModel[];
  listaTiposChavePix: any[];
  existeErro: boolean = false;
  cliId = 0;

  constructor(private fb: FormBuilder,  private clienteService: ClienteService,private bancoService: BancoService,private toastService : ToastService) { }

  ngOnInit() {
    this.cliId = Number(localStorage.getItem(GeralEnum.IDCLIENTE));
    this.listaTiposChavePix = TiposChavePix;
    this.buscarListaBancos();
    this.buscarCliente();
  }

  createForm(_cliente: ClienteModel) {
    this.clienteOld = Object.assign({},_cliente);
   

    this.formulario = this.fb.group({
      id: [_cliente.id],
      banId: [_cliente.banId],
      numAgencia: [_cliente.numAgencia],
      numConta: [_cliente.numConta],
      tipoConta: [_cliente.tipoConta],
      tipoChavePix: [_cliente.tipoChavePix],
      chavePix: [_cliente.chavePix],
    });
  }

  buscarListaBancos() {
    this.bancoService.buscarAtivos().subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
          console.log(res.data);
          return;
        }

        this.listaBancos = res.data;
      },
      error => {
        console.log(error);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
      }
    )
  }

  buscarCliente() {
    this.clienteService.buscaDadosCliente(this.cliId).subscribe(
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

  prepararModel() : ClienteModel {
    const controls = this.formulario.controls;
    const _cliente = new ClienteModel();

    _cliente.banId = controls.banId.value;
    _cliente.numAgencia = controls.numAgencia.value;
    _cliente.numConta = controls.numConta.value;
    _cliente.tipoConta = controls.tipoConta.value;
    _cliente.tipoChavePix = controls.tipoChavePix.value;
    _cliente.chavePix= controls.chavePix.value;
    _cliente.id = this.cliId;

    return _cliente;
  }

  submit(){
    this.existeErro = false;

    let conteudoModelPreparado = this.prepararModel();

    this.alterar(conteudoModelPreparado);
  }

  alterar(model: ClienteModel) {
    this.clienteService.atualizarDadosBancariosCliente(model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar alteração', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Alteração realizado com sucesso!');
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o alteração');
      }
    )
  }

}
