import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DataService, Repository } from '../data.service';

@Component({
  selector: 'repository-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit, OnDestroy {
  repo?: Repository;
  private data?: Subscription;

  constructor(private dataService: DataService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('repoId'));
    this.data = this.dataService.getById(id).subscribe({
      next: repo => {
        if (repo == null) {
          this.router.navigate(['404']);
        } else {
          this.repo = repo;
        }
      },
      error: () => this.router.navigate(['404'])
    });
  }

  ngOnDestroy() {
    this.data?.unsubscribe();
  }
}
