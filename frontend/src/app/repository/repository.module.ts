import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RepositoryRoutingModule } from './repository-routing.module';
import { MatTabsModule } from '@angular/material/tabs';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MainComponent } from './main/main.component';
import { HttpClientModule } from '@angular/common/http';

@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    RepositoryRoutingModule,
    MatTabsModule,
    MatIconModule,
    MatTableModule
  ],
  exports: [
    MainComponent
  ]
})
export class RepositoryModule { }
