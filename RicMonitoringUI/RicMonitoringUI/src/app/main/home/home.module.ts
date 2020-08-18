import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { FuseSharedModule } from '@fuse/shared.module';
import { HomeComponent } from './home.component';

const routes = [
  {
    path        : 'home',
    component   : HomeComponent   
  }
]

@NgModule({
  imports: [
    RouterModule.forChild(routes),
    FuseSharedModule
  ],
  declarations: [HomeComponent],
  exports: [
    HomeComponent
  ]
})
export class HomeModule { }
