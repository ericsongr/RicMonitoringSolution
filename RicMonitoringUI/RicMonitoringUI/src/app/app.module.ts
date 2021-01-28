import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, Routes } from '@angular/router';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { MatButtonModule, MatIconModule } from '@angular/material';
import { TranslateModule } from '@ngx-translate/core';
import 'hammerjs';

import { FuseModule } from '@fuse/fuse.module';
import { FuseSharedModule } from '@fuse/shared.module';
import { FuseProgressBarModule, FuseSidebarModule, FuseThemeOptionsModule } from '@fuse/components';
import { MatSelectCountryModule } from '@angular-material-extensions/select-country';

import { fuseConfig } from 'app/fuse-config';

import { AppComponent } from 'app/app.component';
import { LayoutModule } from 'app/layout/layout.module';
// import { SampleModule } from 'app/main/sample/sample.module';
import { UnauthorizedComponent } from './main/apps/unauthorized/unauthorized.component';
// import { CoreModule } from './main/apps/common/core/core.module';
import { APP_BASE_HREF } from '@angular/common';
import { HomeModule } from './main/home/home.module';
import { ConfigService } from './main/apps/common/services/config.service';
import { UserDataService } from './main/apps/administrator/users/user-data.service';
import { httpInterceptorProviders } from './core/http-interceptors';

import { AuthModule } from './main/apps/auth/auth.module';

import { AuthenticationModule } from './core/auth/authentication.module';
import { NgxPermissionsGuard, NgxPermissionsModule } from 'ngx-permissions';

const appRoutes: Routes = [
    {
        path            : 'auth',
        loadChildren    : () => import('./main/apps/auth/auth.module').then(m => m.AuthModule),
        canActivate: [NgxPermissionsGuard],
		data: {
			permissions: {
				except: ['ADMIN']
			}
		},
    },
    {
        path            : 'apartment',
        loadChildren    : () => import('./main/apps/app-ricmonitoring.module').then(m => m.AppRicMonitoringModule),
        canActivate: [NgxPermissionsGuard],
		data: {
			permissions: {
				only: ['ADMIN', 'USER'],
				// except: ['GUEST'],
				redirectTo: '/auth' // will go to route /auth/login
			}
        },
    },
    {
        path            : 'booking',
        loadChildren    : () => import('./main/apps/online-booking/online-booking.module').then(m => m.OnlineBookingModule)
    },
    {
        path            : 'administrator',
        loadChildren    : () => import('./main/apps/administrator/administrator.module').then(m => m.AdministratorModule),
        canActivate: [NgxPermissionsGuard],
		data: {
			permissions: {
				only: ['ADMIN', 'USER'],
				except: ['GUEST'],
				redirectTo: '/auth' // will go to route /auth/login
			}
        }
    },
    // {
    //   path          : 'auth',
    //   outlet        : 'dialog',
    //   loadChildren  : () => import('./main/apps/administrator/auth/auth.module').then(m => m.AuthModule) //'./main/apps/administrator/auth/change-password'
    // },
    { path: 'unauthorized', component: UnauthorizedComponent },
    { path: 'forbidden', component: UnauthorizedComponent },

   
    {
        path      : '**',
        redirectTo: 'home'
        // redirectTo: 'booking'
    }
];

@NgModule({
    declarations: [
        AppComponent,
        UnauthorizedComponent
    ],
    imports     : [
        BrowserModule,
        BrowserAnimationsModule,
        HttpClientModule,
        RouterModule.forRoot(appRoutes),

        TranslateModule.forRoot(),

        // Material moment date module
        MatMomentDateModule,

        // Material
        MatButtonModule,
        MatIconModule,

        // Fuse modules
        FuseModule.forRoot(fuseConfig),
        MatSelectCountryModule,
        FuseProgressBarModule,
        FuseSharedModule,
        FuseSidebarModule,
        FuseThemeOptionsModule,

        // App modules
        LayoutModule,
        // SampleModule,
        HomeModule,
        // CoreModule, //Note: this module use for oidc authentication
        AuthenticationModule,
		NgxPermissionsModule.forRoot(),
    ],
    providers: [
        ConfigService,
        UserDataService,
        httpInterceptorProviders,
		{
			provide: APP_BASE_HREF,
			useValue: '/'
		},
    ],
    bootstrap   : [AppComponent]
})
export class AppModule { }
