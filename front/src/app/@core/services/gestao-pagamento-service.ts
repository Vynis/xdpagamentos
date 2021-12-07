import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "../../../environments/environment";
import { PaginationFilterModel } from "../models/configuracao/paginationfilter.model";
import { GestaoPagamentoModel } from "../models/gestao-pagamento.model";
import { ModeloBase } from "../models/modelo-balse";

@Injectable()
export class GestaoPagamentoService { 
    
    caminhoApi: string = '';

    constructor(private http: HttpClient) { 
        this.caminhoApi =  `${environment.api}/GestaoPagamento`;
    }

    buscar(filtro: PaginationFilterModel) {
        return this.http.post<ModeloBase>(`${this.caminhoApi}/buscar-gestao-pagamento-filtro`, filtro);
    }

    inserir(model : GestaoPagamentoModel) {
        return this.http.post<ModeloBase>(`${this.caminhoApi}/inserir`, model);
    }

    

}