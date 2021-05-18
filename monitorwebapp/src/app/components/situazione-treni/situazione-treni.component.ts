import { Component, OnInit } from '@angular/core';
import { andamentoTreno } from 'src/app/models/andamentoTreno';
import { DataItem } from 'src/app/models/dataItem';
import { InOutbound } from 'src/app/models/inoutbound';
import { SoluzioniVehicle } from 'src/app/models/soluzioni-vehicles';
import { CommonService } from 'src/app/services/common.service';

@Component({
  selector: 'app-situazione-treni',
  templateUrl: './situazione-treni.component.html',
  styleUrls: ['./situazione-treni.component.css'],
})
export class SituazioneTreniComponent implements OnInit {
  isLoading = false;
  currentDate;
  comboTratte: string[] = [
    'BIELLA - NOVARA',
    'BIELLA - SANTHIA',
    'TORINO P.N. - MILANO C.LE',
    'TORINO P.N. - MILANO P.G.',
  ];
  dataList : DataItem[] = new Array;

  constructor(private commonService: CommonService) {}

  ngOnInit(): void {
    this.currentDate = Date.now();
  }

  OnChangeSelected(item: string) {

    this.getInfoByTrain(item);
  }

  getInfoByTrain(item: string) {
    let trainsList : number[] = new Array()
    this.dataList.length = 0
    let obj = this.getJourneys(item);
    if (obj) {
       this.isLoading = true
        this.commonService.getStations('soluzioniViaggioNew', obj.inbound, obj.outbound, obj.pattern).subscribe((data: SoluzioniVehicle[]) => {
          const regexTrain = new RegExp(obj.pattern)
              data.forEach(sol => {
                sol.soluzioni.forEach((veh) => {
              veh.vehicles.forEach((v) => {
                if (
                  (v.categoria === '235' && v.categoriaDescrizione === 'RV') ||
                  (v.categoria === '197' &&
                    v.categoriaDescrizione === 'Regionale')
                ) {
                  if (!(trainsList.indexOf(+v.numeroTreno) > -1) &&
                  regexTrain.test(v.numeroTreno)) {
                    trainsList.push(+v.numeroTreno);

                    this.commonService.getTrainNumber('cercaNumeroTrenoTrenoAutocomplete', v.numeroTreno).subscribe((data: string) => {
                         let result = data;
                         let splittedString: string
                         splittedString = result.split('\n')[0].split('|')[1]
                         const stationCode = splittedString.split('-')[1]
                         const trainNumber = splittedString.split('-')[0]
                         const time = splittedString.split('-')[2]

                         let results : andamentoTreno
                         this.commonService.getTrainDelay('andamentoTreno', stationCode, trainNumber, time).subscribe((data : andamentoTreno) => {
                           results = data
                            let statoTreno = this.getStatoTreno(results.tipoTreno, results.provvedimento.toString(), results.subTitle)
                           if(results.fermate.length > 0) {
                               results.fermate.forEach(f => {
                                  let y : number = +f.programmata
                                  this.dataList.push(new DataItem(trainNumber, f.stazione, f.ritardo, statoTreno? statoTreno :new Date(y).toLocaleTimeString("it-IT", { hour: '2-digit', minute:'2-digit'}), f.tipoFermata === 'A' || f.tipoFermata === 'P'))
                              })
                            }
                            else {
                              this.dataList.push(new DataItem(trainNumber, '', 0, statoTreno? statoTreno : '', true))
                            }
                            this.dataList.sort((a, b) => {
                              return a.trainNumber > b.trainNumber? 1 : -1
                            })
                         })

                    })
                  }
                }
              });

            });
        });
      });
        this.isLoading = false;


    }
  }

  getStatoTreno (input1: string, input2: string, input3: string ): string{
            if (input1 === 'PG' && input2 === '0')
            {
                return '';
            }
            if (input1 === 'ST' && input2 === '1')
            {
                return 'SOPPRESSO';
            }
            if ((input1 === 'PP' || input1 === 'SI' || input1 === 'SF' || input1 === 'RF') && (input2 === '0' || input2 === '2'))
            {
                return `PARZ. SOPPRESSO - ${input3}`;
            }
            if (input1 === 'DV' && input2 === '3')
            {
                return 'DEVIATO';
            }
            return '';
  }

  getJourneys(item: string): InOutbound {
    switch (item) {
      case 'BIELLA - NOVARA':
        return{
          pattern: '^11[6|7]\\d{2}$',
          inbound: '70',
          outbound: '248',
        };
      case 'BIELLA - SANTHIA':
        return{
          pattern: '^117\\d{2}$',
          inbound: '70',
          outbound: '240',
        };
      case 'TORINO P.N. - MILANO C.LE':
        return{
          pattern: '^^20\\d{2}$',
          inbound: '219',
          outbound: '1700',
        };
      case 'TORINO P.N. - MILANO P.G.':
        return{
          pattern: '^^20\\d{2}$',
          inbound: '219',
          outbound: '1645',
        };
        /*arr.push({
          pattern: '^^2\\d{3}$',
          inbound: '452',
          outbound: '1645', // garibaldi
        });
        arr.push({
          pattern: '^^2\\d{3}$',
          inbound: '452',
          outbound: '248',
        });*/
        // arr.push({
        //   pattern: '^^2\\d{3}$',
        //   inbound: '1700',
        //   outbound: '219',
        // });
        // arr.push({
        //   pattern: '^^2\\d{3}$',
        //   inbound: '1645',
        //   outbound: '452',
        // });
        // arr.push({
        //   pattern: '^^2\\d{3}$',
        //   inbound: '248',
        //   outbound: '452',
        // });
    }
  }
}

