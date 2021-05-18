import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SituazioneTreniComponent } from './components/situazione-treni/situazione-treni.component';
import { SituazioneArriviComponent } from './components/situazione-arrivi/situazione-arrivi.component';
import { HttpService } from './services/http.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { CommonService } from './services/common.service';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    SituazioneTreniComponent,
    SituazioneArriviComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [HttpService, HttpClient, CommonService],
  bootstrap: [AppComponent]
})
export class AppModule { }
