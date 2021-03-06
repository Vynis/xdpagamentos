import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { PaginationFilterModel } from "../models/configuracao/paginationfilter.model";
import { GestaoPagamentoModel } from "../models/gestao-pagamento.model";
import { ModeloBase } from "../models/modelo-balse";

@Injectable()
export class GestaoExtratoService { 
    
    caminhoApi: string = '';

    constructor(private http: HttpClient) { 
        this.caminhoApi =  `${environment.api}/GestaoExtrato`;
    }

    buscar(filtro: PaginationFilterModel) {
        return this.http.post<ModeloBase>(`${this.caminhoApi}/buscar-gestao-extrato-filtro`, filtro);
    }

    inserir(model : GestaoPagamentoModel) {
        return this.http.post<ModeloBase>(`${this.caminhoApi}/inserir`, model);
    }

    remover(id) {
        return this.http.delete<ModeloBase>(`${this.caminhoApi}/excluir/${id}`);
    }    

}