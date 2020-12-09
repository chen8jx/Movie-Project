import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders, HttpParams } from "@angular/common/http";
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})//dependency injection
export class ApiService {

  constructor(protected http: HttpClient) { }

  //genres call genres service, call api service
  getAll(path: string): Observable<any>{
    return this.http
    .get(`${environment.apiUrl}${path}`)
    .pipe(map((resp) => resp as any[]));
  }

  getOne(path: string, id?: number): Observable<any> {
    let getUrl: string;
    if (id) {
      getUrl = `${environment.apiUrl}${path}` + '/' + id;
    } else {
      getUrl = `${environment.apiUrl}${path}`;
    }
    return this.http.get(getUrl).pipe(map((resp) => resp as any));
  } 
}
