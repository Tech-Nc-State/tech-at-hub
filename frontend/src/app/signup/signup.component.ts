import { Component, OnInit } from '@angular/core';
import { User } from './user';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  userModel = new User('', '', '', '');
  
  submit(){


     this.userModel.username = (document.getElementById("name") as HTMLInputElement).value

     this.userModel.password = (document.getElementById("password-field") as HTMLInputElement).value

     this.userModel.email = (document.getElementById("email") as HTMLInputElement).value

     this.userModel.dob = (document.getElementById("dob") as HTMLInputElement).value

    console.log(this.userModel)

  }
  

}
