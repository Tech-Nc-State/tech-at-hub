import { Injectable } from '@angular/core';
import { Repository } from './data.service';

@Injectable({
  providedIn: 'root'
})
export class RepositoryService {
  private repo?: Repository;

  constructor() { }

  getRepository() {
    return this.repo;
  }

  updateRepository(repo: Repository) {
    this.repo = repo;
  }
}
