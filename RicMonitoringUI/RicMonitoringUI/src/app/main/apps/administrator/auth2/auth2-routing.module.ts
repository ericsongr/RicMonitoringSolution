import { NgModule } from '@angular/core';
import { AuthComponent } from './auth2.component';
import { RouterModule } from '@angular/router';
import { Route } from '@angular/router';

const routes: Route[] = [
  {
    component: AuthComponent,
    path: '',
    children: [
      {
        path: 'change-password',
        loadChildren: () => import('../auth2/change-password/change-password.module').then(m => m.ChangePasswordModule)
      },
      {
        path: '**',
        redirectTo: 'change-password'
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
