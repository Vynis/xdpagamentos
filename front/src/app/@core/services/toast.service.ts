import { Injectable } from '@angular/core';
import { NbComponentStatus, NbToastrService } from '@nebular/theme';

@Injectable({
  providedIn: 'root'
})
export class ToastService {

  constructor(private toastrService: NbToastrService) { }

  showToast(status: NbComponentStatus, titulo, msg = '') {
    this.toastrService.show(msg, titulo, { status });
  }
}
