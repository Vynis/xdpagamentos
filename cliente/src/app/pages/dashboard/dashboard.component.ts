import {Component, OnDestroy} from '@angular/core';
import { NbThemeService } from '@nebular/theme';
import { takeWhile } from 'rxjs/operators' ;
import { SolarData } from '../../@core/data/solar';
import { SessoesEnum } from '../../@core/enums/sessoes.enum';
import { AuthServiceService } from '../../@core/services/auth-service.service';
import { RelatoriosService } from '../../@core/services/relatorios.service';
import { GeralEnum } from '../../@core/enums/geral.enum';

interface CardSettings {
  title: string;
  iconClass: string;
  type: string;
}

@Component({
  selector: 'ngx-dashboard',
  styleUrls: ['./dashboard.component.scss'],
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnDestroy {
  options: any;
  listaDatas: string[];
  listaValores: number[];
  loading = false;
  cliId= 0;

  constructor(private themeService: NbThemeService,
              private solarService: SolarData,
              private authService: AuthServiceService,
              private relatorioService: RelatoriosService
              ) {


  }

  ngOnInit(): void {
    setTimeout(() => {
      this.cliId = Number(localStorage.getItem(GeralEnum.IDCLIENTE));
      this.buscaDadosGraficoVendas();
    }, 3000);
    
  }

  ngOnDestroy() {

  }

  buscaDadosGraficoVendas() {
    this.loading = true;
    this.relatorioService.buscaGraficoVendas(this.cliId).subscribe(
      res => {
        if (!res.success)
          return; 

        this.listaDatas = res.data.listaDatas;
        this.listaValores = res.data.listaValores;
        this.carregaGraficoVendas();
        this.loading = false;
      }
    )
  }

  carregaGraficoVendas() {
    this.options = {
      color: ['#3398DB'],
      tooltip: {
        trigger: 'axis',
        axisPointer: {
          type: 'shadow'
        }
      },
      grid: {
        left: '3%',
        right: '4%',
        bottom: '3%',
        containLabel: true
      },
      xAxis: [
        {
          type: 'category',
          data: this.listaDatas,
          axisTick: {
            alignWithLabel: true
          }
        }
      ],
      yAxis: [{
        type: 'value'
      }],
      series: [{
        name: 'Qtd. vendas',
        type: 'bar',
        barWidth: '60%',
        data: this.listaValores
      }]
    };
    
  }


}
