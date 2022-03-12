import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { DataService, Repository } from '../data.service';
import { RepositoryService } from '../repository.service';

interface Tab {
  name: string;
  icon: string;
  path: string;
}

@Component({
  selector: 'repository-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {
  data$?: Observable<Repository>;
  tabs: Tab[] = [
    { name: 'Code', icon: 'code', path: 'code' },
    { name: 'Issues', icon: 'bug_report', path: 'issues' },
    { name: 'Pull Requests', icon: 'call_merge', path: 'prs' }
  ];
  active = this.tabs[0];

  constructor(private dataService: DataService,
    private repositoryService: RepositoryService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('repoId'));
    this.data$ = this.dataService.getById(id)
      .pipe(
        tap(repo => {
          this.repositoryService.updateRepository(repo);
        }),
        catchError(err => {
          this.router.navigate(['404']);
          return throwError(() => new Error(err));
        })
      );
  }
}
