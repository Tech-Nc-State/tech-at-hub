import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RepositoryRoutingModule } from './repository-routing.module';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { MainComponent } from './main/main.component';
import { HttpClientModule } from '@angular/common/http';
import { ExplorerComponent } from './explorer/explorer.component';

@NgModule({
  declarations: [
    MainComponent,
    ExplorerComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    RepositoryRoutingModule,
    MatTabsModule,
    MatIconModule
  ],
  exports: [
    MainComponent
  ]
})
export class RepositoryModule { }
