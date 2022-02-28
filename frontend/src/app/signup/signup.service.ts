import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SignupService {

  constructor() { }

  Save(val: any){
    console.log(val)
  }
}
