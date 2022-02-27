import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { DataService, Repository } from '../data.service';

export interface File {
  name: string;
  commit: string;
}

const ELEMENT_DATA: File[] = [
  { name: 'Hydrogen', commit: 'Commit 1' },
  { name: 'Hydrogen', commit: 'Commit 1' },
  { name: 'Hydrogen', commit: 'Commit 1' },
  { name: 'Hydrogen', commit: 'Commit 1' },
  { name: 'Hydrogen', commit: 'Commit 1' },
  { name: 'Hydrogen', commit: 'Commit 1' },
  { name: 'Hydrogen', commit: 'Commit 1' },
];

@Component({
  selector: 'repository-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {
  displayedColumns: string[] = ['name', 'commit'];
  dataSource = ELEMENT_DATA;
  id?: number;
  data$?: Observable<Repository | undefined>;

  constructor(private dataService: DataService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    const idstr = this.route.snapshot.paramMap.get('repoId');
    const id = Number(idstr);
    if (isNaN(id)) {
      this.router.navigate(['404']);
    }

    this.data$ = this.dataService.getById(id);
  }
}
