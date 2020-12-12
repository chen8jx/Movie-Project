import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Login } from 'src/app/shared/models/login';
import { ApiService } from './api.service';
import { JwtStorageService } from './jwt-storage.service';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private apiService: ApiService,private jwtStorageService: JwtStorageService) { }

  //login component will call this one
  login(userLogin: Login): Observable<boolean> {
    return this.apiService.create('account/login', userLogin).pipe(
      map((response) => {
        if (response) {
          console.log(response);
          // once we get the JWT token from API,  Angular will save that token in local storage
          this.jwtStorageService.saveToken(response.token);
          // then decode that token and fill up User object
          return true;
        }
        return false;
      })
    );
  }
  //sign up component will call this
  register(){

  }

  //from header when click logout
  logout(){

  }
}
