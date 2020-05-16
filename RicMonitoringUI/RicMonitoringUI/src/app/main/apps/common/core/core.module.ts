import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthModule, OidcSecurityService } from 'angular-auth-oidc-client';
import { AuthService } from './auth/auth.service';

@NgModule({
  imports: [
    CommonModule,
    AuthModule.forRoot(),
  ],
  declarations: [],
  providers: [
    AuthService,
    OidcSecurityService
  ]
})
export class CoreModule { }
