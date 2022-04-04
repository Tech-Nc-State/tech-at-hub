import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExplorerComponent } from './explorer/explorer.component';
import { MainComponent } from './main/main.component';

const routes: Routes = [
  {
    path: ':repoId',
    component: MainComponent,
    children: [
      { path: '**', component: ExplorerComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RepositoryRoutingModule { }
