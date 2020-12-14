import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
  returnUrl:string;
  constructor(private authService: AuthenticationService, private router:Router,private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.queryParams.subscribe(
      (params) => (this.returnUrl = params.returnUrl || '/')
    );
  }

  login(){
    this.authService.login(this.userLogin).subscribe((response)=>{
      if(response){
        //redirect one page
        this.router.navigate([this.returnUrl]);
      }
    },
    (error: any)=>{
      this.invalidLogin=true;
    })
  }
}
