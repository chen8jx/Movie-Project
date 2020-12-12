import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/core/services/authentication.service';
import { Login } from 'src/app/shared/models/login';


//Angular View (Login) will send the object info to Angular (Login) Component
//ANgular Component will send that object to Angular Service (Auth Service)
//Auth Service will send to Angular API Service, that will call the actual POST REST API (Account/login method)
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  userLogin: Login={
    email: '',
    password: ''
  };

  invalidLogin: boolean = false;

  constructor(private authService: AuthenticationService) { }

  ngOnInit() {
  }

  login(){
    this.authService.login(this.userLogin).subscribe((response)=>{
      if(response){
        //redirect one page
      }
    },
    (error: any)=>{
      this.invalidLogin=true;
    })
  }
}
