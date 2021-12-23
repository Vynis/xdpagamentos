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

    inserir(model : GestaoPagamentoModel) {
        return this.http.post<ModeloBase>(`${this.caminhoApi}/inserir`, model);
    }

    remover(id) {
        return this.http.delete<ModeloBase>(`${this.caminhoApi}/excluir/${id}`);
    }

    saldoAtual() {
        return this.http.get<ModeloBase>(`${this.caminhoApi}/saldo-atual`);
    }

    

}