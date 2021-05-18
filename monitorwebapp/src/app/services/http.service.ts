import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
  HttpResponse,
} from '@angular/common/http';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class HttpService {
  baseUrl = 'https://thingproxy.freeboard.io/fetch/http://www.viaggiatreno.it/viaggiatrenonew/resteasy/viaggiatreno/';
  constructor(private http: HttpClient) {}

  public get( path, responseType): Observable<any> {
    const url = this.baseUrl + path;
    const headers = new HttpHeaders();
      return this.http.get(url, {headers: headers, responseType: responseType} ).pipe(
        catchError(this.handleError)
      );
    }

    private handleError(error: HttpErrorResponse) {
      if (error.status === 0) {
        // A client-side or network error occurred. Handle it accordingly.
        console.error('An error occurred:', error.error);
      } else {
        // The backend returned an unsuccessful response code.
        // The response body may contain clues as to what went wrong.
        console.error(
          `Backend returned code ${error.status}, ` +
          `body was: ${error.error}`);
      }
      // Return an observable with a user-facing error message.
      return throwError(
        'Something bad happened; please try again later.');
    }
  }

