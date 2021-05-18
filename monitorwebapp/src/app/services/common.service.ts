import { formatDate } from '@angular/common';
import { Injectable } from '@angular/core';
import { concat, forkJoin, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root'
})
export class CommonService {

  constructor(private httpService : HttpService) { }

  getStations(path: string, inbound: string, outbound: string, pattern: string ) : Observable<any> {
    return forkJoin([
       this.httpService.get(path +'/'+ inbound + '/'  + outbound + '/' + formatDate(Date.now(), 'yyyy-MM-ddT00:00:00', 'en-US'), 'json'),
       this.httpService.get(path +'/'+ outbound + '/'  + inbound + '/' + formatDate(Date.now(), 'yyyy-MM-ddT00:00:00', 'en-US'), 'json')
    ])
  }

  getTrainNumber(path: string, number: string ) : Observable<string> {
    return this.httpService.get(path +'/'+ number, 'text');
  }

  getTrainDelay(path: string, stationCode: string, number: string, time:string ) : Observable<any> {
    return this.httpService.get(path +'/'+ stationCode + '/' + number + '/' + time, 'json');
  }
}
