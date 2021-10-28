import { ClienteService } from './../../../@core/services/cliente.service';
import { ClienteModel } from './../../../@core/models/cliente.model';
import { TerminalService } from './../../../@core/services/terminal.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastPadrao } from '../../../@core/enums/toast.enum';
import { EstabelecimentoModel } from '../../../@core/models/estabelecimento.model';
import { TerminalModel } from '../../../@core/models/terminal.model';
import { EstabelecimentoService } from '../../../@core/services/estabelecimento.service';
import { ToastService } from '../../../@core/services/toast.service';
import { AuthServiceService } from '../../../@core/services/auth-service.service';
import { SessoesEnum } from '../../../@core/enums/sessoes.enum';

@Component({
  selector: 'ngx-terminal-cadastro',
  templateUrl: './terminal-cadastro.component.html',
  styleUrls: ['./terminal-cadastro.component.scss']
})
export class TerminalCadastroComponent implements OnInit {

  tituloPagina: string = 'Cadastro de Terminais';
  existeErro: boolean = false;
  formulario: FormGroup;
  terminalOld: TerminalModel;
  listaEstabelecimentos: EstabelecimentoModel[];
  listaClientes: ClienteModel[];

  constructor(
    private fb: FormBuilder,
    private route : Router,
    private activatedRoute: ActivatedRoute,
    private estabelecimentoService: EstabelecimentoService,
    private terminalService: TerminalService,
    private clienteService: ClienteService,
    private toastService : ToastService,
    private authService: AuthServiceService
  ) { }

  ngOnInit(): void {
    this.buscarListaEstabelecimentos();
    this.buscaListaClientes();

    this.activatedRoute.params.subscribe(params => {
      const id = params.id;
      if (id && id > 0) {
        this.authService.validaPermissaoTela(SessoesEnum.ALTERAR_POS);
        this.tituloPagina = `Editar Terminal - Nº ${id}`;
        this.buscaPorId(id);
      }
      else {
        this.authService.validaPermissaoTela(SessoesEnum.CADASTRO_POS);
        const model = new TerminalModel();
        this.createForm(model);
      }
    });
  }

  createForm(_terminal: TerminalModel) {
    this.terminalOld = Object.assign({},_terminal);

    this.formulario = this.fb.group({
      id: [_terminal.id],
      numTerminal: [_terminal.numTerminal, Validators.required],
      estId: [_terminal.estId, Validators.required],
      status: [_terminal.status, Validators.required],
      cliId: [ _terminal.listaRelClienteTerminal.length > 0 ? _terminal.listaRelClienteTerminal[0].cliId : 0, Validators.required]
    });
  }

  buscarListaEstabelecimentos() {
    this.estabelecimentoService.buscarAtivos().subscribe(
      res => {
        
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
          console.log(res.data);
          return;
        }

        this.listaEstabelecimentos = res.data;

      },
      error => {
        console.log(error);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
      }
      
    )
  }

  buscaListaClientes() {
    this.clienteService.buscarAtivos().subscribe(
      res => {

        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
          console.log(res.data);
          return;
        }
        
        this.listaClientes = res.data;

      },
      error => {
        console.log(error);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
      }
    )
  }

  buscaPorId(id: number) {
    this.terminalService.buscaPorId(id).subscribe(
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
    this.route.navigateByUrl('/pages/terminal');
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

  prepararModel() : TerminalModel {
    const controls = this.formulario.controls;
    const _terminal = new TerminalModel();

    _terminal.id = controls.id.value == null ? 0 : controls.id.value;
    _terminal.numTerminal = controls.numTerminal.value;
    _terminal.estId = controls.estId.value;
    _terminal.status = controls.status.value;
    _terminal.listaRelClienteTerminal.push({ id:0, cliId: controls.cliId.value, terId: 0 });

    return _terminal;
  }


  inserir(model : TerminalModel) {
    this.terminalService.inserir(model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Cadastro realizado com sucesso!');
        this.route.navigateByUrl('/pages/terminal');
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro');
      }
    )
  }

  alterar(model: TerminalModel) {
    this.terminalService.alterar(model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar alteração', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Alteração realizado com sucesso!');
        this.route.navigateByUrl('/pages/terminal');
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o alteração');
      }
    )
  }

}
