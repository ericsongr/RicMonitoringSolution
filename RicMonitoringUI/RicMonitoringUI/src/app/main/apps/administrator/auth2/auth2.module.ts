import { NgModule } from '@angular/core';
import { AuthComponent } from './auth2.component';
import { AuthRoutingModule } from './auth2-routing.module';

@NgModule({
  declarations: [AuthComponent],
  imports: [
    AuthRoutingModule
  ],
  exports: [AuthComponent]
})
export class Auth2Module { }
