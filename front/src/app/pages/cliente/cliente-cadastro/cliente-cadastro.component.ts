import { TipoTransacaoService } from './../../../@core/services/tipo-transacao.service';
import { TipoTransacaoModel } from './../../../@core/models/tipo-transacao.model';
import { BancoModel } from './../../../@core/models/banco.model';
import { BancoService } from './../../../@core/services/banco.service';
import { EstabelecimentoModel } from './../../../@core/models/estabelecimento.model';
import { EstabelecimentoService } from './../../../@core/services/estabelecimento.service';
import { ClienteModel } from './../../../@core/models/cliente.model';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray } from '@angular/forms';
import { EstadosBrasileiros } from '../../../@core/enums/estados-brasileiros.enum';
import { ClienteService } from '../../../@core/services/cliente.service';
import { ToastService } from '../../../@core/services/toast.service';
import { ToastPadrao } from '../../../@core/enums/toast.enum';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthServiceService } from '../../../@core/services/auth-service.service';
import { SessoesEnum } from '../../../@core/enums/sessoes.enum';
import { OrderPipe } from 'ngx-order-pipe';
import { formatarNumero, formatarNumeroUS } from '../../../@core/utils/funcoes';
import { GenericValidator } from '../../../@core/utils/generic-validator';

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
  listaEstabelecimentos: EstabelecimentoModel[];
  listaBancos: BancoModel[];
  listaTaxas: TipoTransacaoModel[];

  get taxas(): FormArray{
    return <FormArray>this.formulario.get('taxas');
  }

  constructor(
    private fb: FormBuilder,
    private route : Router,
    private activatedRoute: ActivatedRoute,
    private estabelecimentoService: EstabelecimentoService,
    private clienteService: ClienteService,
    private bancoService: BancoService,
    private tipoTransacaoService: TipoTransacaoService,
    private toastService : ToastService,
    private authService: AuthServiceService,
    private orderPipe: OrderPipe
    ) { }

  ngOnInit(): void {
    this.listaestadosBrasileiros = EstadosBrasileiros;
    this.buscarListaEstabelecimentos();
    this.buscarListaBancos();

    this.activatedRoute.params.subscribe(params => {
      const id = params.id;
      if (id && id > 0) {
        this.authService.validaPermissaoTela(SessoesEnum.ALTERAR_CLIENTES);
        this.tituloPagina = `Editar Cliente - Nº ${id}`;
        this.buscaPorId(id);
      }
      else {
        this.authService.validaPermissaoTela(SessoesEnum.CADASTRO_CLIENTES);
        this.buscarTipoTransacao();
        const model = new ClienteModel();
        this.createForm(model);
      }
    });
    
  }

  createForm(_cliente: ClienteModel) {
    this.clienteOld = Object.assign({},_cliente);

    this.formulario = this.fb.group({
      id: [_cliente.id],
      tipoPessoa: [_cliente.tipoPessoa, Validators.required],
      estId: [_cliente.estId, Validators.required],
      nome: [_cliente.nome, Validators.required],
      cnpjCpf: [_cliente.cnpjCpf,[GenericValidator.isValidCpfCnpj()] ],
      endereco: [_cliente.endereco, Validators.required],
      cep: [_cliente.cep, Validators.required],
      bairro: [_cliente.bairro, Validators.required],
      cidade: [_cliente.cidade, Validators.required],
      estado: [_cliente.estado, Validators.required],
      fone1: [_cliente.fone1],
      fone2: [_cliente.fone2, Validators.required],
      email: [_cliente.email, [Validators.email]],  
      status: [_cliente.status, Validators.required],
      possuiDadosBancario: [ _cliente.banId > 0 ? true : false , Validators.required],
      banId: [_cliente.banId],
      numAgencia: [_cliente.numAgencia],
      numConta: [_cliente.numConta],
      tipoConta: [_cliente.tipoConta],
      limiteCredito: [_cliente.limiteCredito, Validators.required],
      taxas: this.fb.array([this.criaGrupoTaxa()])
    });
  }

  criaGrupoTaxa(taxas: TipoTransacaoModel = new TipoTransacaoModel()) : FormGroup {
    return this.fb.group({
      id: [taxas.id],
      qtdParcelas: [taxas.qtdParcelas],
      percDesconto: [taxas.percDesconto, Validators.required],
      status: [taxas.status],
      cliId : [taxas.cliId]
    })
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

  buscarTipoTransacao() {
    this.tipoTransacaoService.buscarPadrao().subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
          console.log(res.data);
          return;
        }

        this.listaTaxas = <TipoTransacaoModel[]>res.data;

        this.taxas.removeAt(0);

        (<TipoTransacaoModel[]>res.data).forEach(taxa => {
          this.taxas.push(this.criaGrupoTaxa(taxa));
        })
      }
    )
  }

  buscaPorId(id: number) {
    this.clienteService.buscaPorId(id).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao buscar dados');
          console.log(res.data);
          return;
        }

        this.listaTaxas = this.orderPipe.transform((<ClienteModel>res.data).listaTipoTransacao, 'qtdParcelas');

        res.data.limiteCredito = formatarNumeroUS(res.data.limiteCredito);

        this.createForm(res.data);


        this.taxas.removeAt(0);

        this.listaTaxas.forEach(taxa => {
          this.taxas.push(this.criaGrupoTaxa(taxa));
        })
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
    this.route.navigateByUrl('/pages/cliente');
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

    // if (!this.ehNumeric(controls.limiteCredito.value)) {
    //   this.existeErro = true;
    //   return false;
    // }

    return true;
  }


  prepararModel() : ClienteModel {
    const controls = this.formulario.controls;
    const _cliente = new ClienteModel();

    _cliente.id = controls.id.value == null ? 0 : controls.id.value;
    _cliente.tipoPessoa = controls.tipoPessoa.value;
    _cliente.estId = controls.estId.value;
    _cliente.cnpjCpf = controls.cnpjCpf.value;
    _cliente.nome = controls.nome.value;
    _cliente.cep = controls.cep.value;
    _cliente.endereco = controls.endereco.value;
    _cliente.bairro = controls.bairro.value;
    _cliente.cidade = controls.cidade.value;
    _cliente.estado = controls.estado.value;
    _cliente.fone1 = controls.fone1.value;
    _cliente.fone2 = controls.fone2.value;
    _cliente.email = controls.email.value;
    _cliente.status = controls.status.value;
    _cliente.limiteCredito = formatarNumero(controls.limiteCredito.value);

    controls.taxas.value.forEach(taxa => {
      _cliente.listaTipoTransacao.push({id: 0, percDesconto: taxa.percDesconto, qtdParcelas: taxa.qtdParcelas, status: taxa.status, cliId: _cliente.id})
    });

    if (controls.possuiDadosBancario.value) {

      _cliente.banId = controls.banId.value;
      _cliente.numAgencia = controls.numAgencia.value;
      _cliente.numConta = controls.numConta.value;
      _cliente.tipoConta = controls.tipoConta.value;

    }

    return _cliente;
  }


  inserir(model : ClienteModel) {
    this.clienteService.inserir(model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Cadastro realizado com sucesso!');
        this.route.navigateByUrl('/pages/cliente');
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o cadastro');
      }
    )
  }

  alterar(model: ClienteModel) {
    this.clienteService.alterar(model).subscribe(
      res => {
        if (!res.success) {
          this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar alteração', res.data);
          return;
        }

        this.toastService.showToast(ToastPadrao.SUCCESS, 'Alteração realizado com sucesso!');
        this.route.navigateByUrl('/pages/cliente');
      },
      erro => {
        console.log(erro);
        this.toastService.showToast(ToastPadrao.DANGER, 'Erro ao realizar o alteração');
      }
    )
  }

  selecaoPossuiContaBancaria(dados){

    const controls = this.formulario.controls;

    controls.banId.setValue('');
    controls.numAgencia.setValue('');
    controls.numConta.setValue('');
    controls.tipoConta.setValue('');

    if (dados) {
      controls.banId.setValidators(Validators.required);
      controls.numAgencia.setValidators(Validators.required);
      controls.numConta.setValidators(Validators.required);
      controls.tipoConta.setValidators(Validators.required);
    }
    else {
      controls.banId.clearValidators();
      controls.numAgencia.clearValidators();
      controls.numConta.clearValidators();
      controls.tipoConta.clearValidators();
    }

    controls.banId.updateValueAndValidity();
    controls.numAgencia.updateValueAndValidity();
    controls.numConta.updateValueAndValidity();
    controls.tipoConta.updateValueAndValidity();

  }

  ehNumeric(value) {
    return /^\d+(?:\,\d+)?$/.test(value);
  }

}
