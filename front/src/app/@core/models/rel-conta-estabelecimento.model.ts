import { ContaCaixaModel } from './contacaixa.model';
import { EstabelecimentoModel } from './estabelecimento.model';

export class RelContaEstabelecimentoModel {
    id: number;
    cocId: number;
    contaCaixa: ContaCaixaModel;
    estId: number;
    estabelecimento: EstabelecimentoModel;
    creditoAutomatico: string = '';
}