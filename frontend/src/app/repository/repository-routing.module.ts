import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExplorerComponent } from './explorer/explorer.component';
import { IssuesComponent } from './issues/issues.component';
import { MainComponent } from './main/main.component';
import { PullRequestsComponent } from './pull-requests/pull-requests.component';

const routes: Routes = [
  { 
    path: ':repoId',
    component: MainComponent,
    children: [
      { path: 'code/:fileId', component: ExplorerComponent },
      { path: 'code', component: ExplorerComponent },
      { path: 'issues', component: IssuesComponent },
      { path: 'prs', component: PullRequestsComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RepositoryRoutingModule { }
