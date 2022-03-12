import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { retry } from 'rxjs';

export interface File {
  id: number;
  name: string;
  files?: File[];
}

export interface Repository {
  id: number;
  name: string;
  files: File[];
}

@Injectable({
  providedIn: 'root'
})
export class DataService {
  constructor(private http: HttpClient) { }

  getById(id: number) {
    return this.http.get<Repository>(`/assets/repo/${id}.json`).pipe(retry(3));
  }
}
