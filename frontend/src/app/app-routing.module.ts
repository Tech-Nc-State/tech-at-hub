import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SignupComponent } from './signup/signup.component';
import { NotFoundComponent } from './not-found/not-found.component';

const routes: Routes = [
  { path: 'repo', loadChildren: () => import('./repository/repository.module').then(m => m.RepositoryModule) },
  {path: 'signup', component: SignupComponent},
  {path: '', redirectTo: 'signup', pathMatch: 'full'},
  { path: '**', component: NotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
