import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';

@Injectable({
  providedIn: 'root'
})
export class SweetalertService {

  constructor() { }

  msgDeletarRegistro(msgTitulo = '', msgTexto = '') {
   return Swal.fire({
      title: msgTitulo == '' ?  'Tem certeza que deseja excluir este registro?' : msgTitulo,
      text: msgTexto == '' ? 'Apos excluir este registro ele não sera capaz de recupera-lo' : msgTexto ,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sim',
      cancelButtonText: 'Não'
    })
  }

  msgAvulsa(msgTitulo, icon, msgTexto) {
    return Swal.fire(msgTitulo, msgTexto, icon);
  }

}