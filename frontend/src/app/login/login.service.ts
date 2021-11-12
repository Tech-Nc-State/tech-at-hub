import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';

export class LoginForm {
  id!: string;
  pass!: string;
}

class User{
  id!: (number)
  userName!: (string)
  email!: (string)
  firstName!: (string)
  lastName!: (string)
  age!: (number)
  description!: (string)
  picture!: (string)
}

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  // TODO: add address of endpoint - 'http://localhost:3000'
  endpoint = ''

  constructor(private httpClient: HttpClient) { }
}
