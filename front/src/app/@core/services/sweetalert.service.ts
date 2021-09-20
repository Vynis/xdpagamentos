import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';
import { SweetAlertIcons } from '../enums/sweet-alert-icons-enum';

@Injectable({
  providedIn: 'root'
})
export class SweetalertService {

  constructor() { }

  msgPadrao(msgTitulo = '', msgTexto = '', icons = SweetAlertIcons.WARNING) {
   return Swal.fire({
      title: msgTitulo == '' ?  'Tem certeza que deseja excluir este registro?' : msgTitulo,
      text: msgTexto == '' ? 'Apos excluir este registro ele não sera capaz de recupera-lo' : msgTexto ,
      icon: icons,
      showCancelButton: true,
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não'
    })
  }

  msgAvulsa(msgTitulo, icon: SweetAlertIcons, msgTexto) {
    return Swal.fire(msgTitulo, msgTexto, icon);
  }

}