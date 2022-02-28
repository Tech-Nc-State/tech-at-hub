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

  userModel = new User('user', 'password', 'user@test.com', '00/00/0000');
  
  

}
