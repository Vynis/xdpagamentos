import { FiltroItemModel } from "./filtroitem.model";

export class PaginationFilterModel {
    take: number;
    skip: number;
    filtro: FiltroItemModel[] = [];
}