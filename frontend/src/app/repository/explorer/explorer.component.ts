import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { File, Repository } from '../data.service';
import { RepositoryService } from '../repository.service';

@Component({
  selector: 'repository-explorer',
  templateUrl: './explorer.component.html',
  styleUrls: ['./explorer.component.scss']
})
export class ExplorerComponent implements OnInit {
  repo!: Repository;
  fileId?: number;
  file?: File;
  cols = ["name"];

  constructor(private repositoryService: RepositoryService,
    private route: ActivatedRoute,
    private router: Router) { }

  ngOnInit() {
    this.repo = this.repositoryService.getRepository() as Repository;
    this.route.params.subscribe(params => {
      if (params['fileId']) {
        this.file = this.findFile(this.repo.files, Number(params['fileId']));
        if (!this.file) {
          this.router.navigate(['404']);
        }
      } else {
        this.file = {
          id: 0,
          name: 'root',
          files: this.repo.files
        };
      }
    })
  }

  private findFile(current: File[], id: number): File | undefined {
    let target = current.find(f => f.id === id);
    if (target) {
      return target;
    }

    for (const file of current) {
      if (file.files) {
        target = this.findFile(file.files, id);
        if (target) {
          return target;
        }
      }
    }

    return target;
  }
}
