import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-situazione-arrivi',
  templateUrl: './situazione-arrivi.component.html',
  styleUrls: ['./situazione-arrivi.component.css']
})
export class SituazioneArriviComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
    console.log('I am situazione-arrivi');
  }

}
