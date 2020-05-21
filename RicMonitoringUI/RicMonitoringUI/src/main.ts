import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from 'app/app.module';
import { environment } from 'environments/environment';
import { hmrBootstrap } from 'hmr';

if ( environment.production )
{
    enableProdMode();
}
/* 
    TODO: currently when development running only with firefox need some fix with chrome
*/

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
  }

const providers = [
   { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] },
     { provide: 'AUTH_URL', useValue: 'https://localhost:5002' },
     { provide: 'API_URL', useValue: 'https://localhost:5001/api/' },
    // { provide: 'AUTH_URL', useValue: 'https://authserver.ericsonramos.com' },
    // { provide: 'API_URL', useValue: 'https://tenantsapi.ericsonramos.com/api/' }
  ];

const bootstrap = () => platformBrowserDynamic(providers).bootstrapModule(AppModule);

if ( environment.hmr )
{
    if ( module['hot'] )
    {
        hmrBootstrap(module, bootstrap);
    }
    else
    {
        console.error('HMR is not enabled for webpack-dev-server!');
        console.log('Are you using the --hmr flag for ng serve?');
    }
}
else
{
    bootstrap().catch(err => console.log(err));
}


  

  
//   platformBrowserDynamic(providers).bootstrapModule(AppModule)
//     .catch(err => console.error(err));
  