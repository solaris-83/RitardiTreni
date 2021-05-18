import { Component, OnInit, ViewEncapsulation} from '@angular/core';
import { SoluzioniVehicle } from './models/soluzioni-vehicles';
import { HttpService } from './services/http.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  encapsulation: ViewEncapsulation.None
  constructor(private httpService: HttpService) {}
  solutions : SoluzioniVehicle [];
  ngOnInit(): void {

  }




}
