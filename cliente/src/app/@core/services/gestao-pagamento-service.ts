import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { GestaoPagamentoData } from "../data/gestao-pagto";
import { PaginationFilterModel } from "../models/configuracao/paginationfilter.model";
import { GestaoPagamentoModel } from "../models/gestao-pagamento.model";
import { ModeloBase } from "../models/modelo-balse";

@Injectable()
export class GestaoPagamentoService extends GestaoPagamentoData { 
    
    caminhoApi: string = '';

    constructor(private http: HttpClient) { 
        super();
        this.caminhoApi =  `${environment.api}/GestaoPagamento`;
    }

    buscar(filtro: PaginationFilterModel) {
        return this.http.post<ModeloBase>(`${this.caminhoApi}/buscar-gestao-pagamento-filtro-cliente`, filtro);
    }
    
    saldoAtual(id: number) {
        return this.http.get<ModeloBase>(`${this.caminhoApi}/saldo-atual-cliente/${id}`);
    }

    solicitarPagto(model: GestaoPagamentoModel) {
        return this.http.post<ModeloBase>(`${this.caminhoApi}/solicitar-pagto-cliente`, model);
    }

}