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
import { SampleModule } from 'app/main/sample/sample.module';
import { UnauthorizedComponent } from './main/apps/unauthorized/unauthorized.component';
import { CoreModule } from './main/apps/common/core/core.module';
import { httpInterceptorProvider } from './main/apps/common/core/http-interceptor/index';
import { APP_BASE_HREF } from '@angular/common';

const appRoutes: Routes = [
    {
        path      : '',
        loadChildren: './main/apps/app-ricmonitoring.module#AppRicMonitoringModule'
    },
    {
        path          : 'booking',
        loadChildren  : './main/apps/online-booking/online-booking.module#OnlineBookingModule'
      },
      { path: 'unauthorized', component: UnauthorizedComponent },
      { path: 'forbidden', component: UnauthorizedComponent },
    {
        path      : '**',
        redirectTo: 'sample'
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
        SampleModule,
        CoreModule
    ],
    providers: [
        // httpInterceptorProvider, 
        // {
        //     provide: APP_BASE_HREF,
        //     useValue: '/'
        // }
    ],
      bootstrap   : [
        AppComponent
    ]
})
export class AppModule
{
}
