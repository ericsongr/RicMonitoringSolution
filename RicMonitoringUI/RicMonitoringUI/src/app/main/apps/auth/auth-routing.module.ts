import { NgModule } from '@angular/core';
import { AuthComponent } from './auth.component';
import { RouterModule } from '@angular/router';
import { Route } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { ResetPasswordComponent } from './reset-password/reset-password.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';

const routes: Route[] = [
  {
    component: AuthComponent,
    path: '',
    children: [
      {
        path     : 'login',
        component: LoginComponent
      },
      {
        path     : 'register',
        component: RegisterComponent
      },
      {
        path     : 'reset-password',
        component: ResetPasswordComponent
      },
      {
        path     : 'forgot-password',
        component: ForgotPasswordComponent
      },
      {
        path: '**',
        redirectTo: 'login'
      }
    ]
  } 
];

@NgModule({
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
