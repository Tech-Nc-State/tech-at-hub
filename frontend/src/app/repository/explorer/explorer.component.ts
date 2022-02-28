import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'repository-explorer',
  templateUrl: './explorer.component.html',
  styleUrls: ['./explorer.component.scss']
})
export class ExplorerComponent implements OnInit {
  paths?: string[];

  constructor(private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.url.subscribe(segments => {
      this.paths = segments.map(segment => segment.path);
    });
  }
}
