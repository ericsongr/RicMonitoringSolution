import { Component, Inject, OnInit} from '@angular/core';
import { FuseConfigService } from '@fuse/services/config.service';
import { FuseNavigationService } from '@fuse/components/navigation/navigation.service';

import { FuseSplashScreenService } from '@fuse/services/splash-screen.service';
import { ApiControllers } from 'environments/api-controllers';
import { environment } from 'environments/environment';
import { AuthenticationService } from 'app/core/auth/authentication.service';
import { TokenStorage } from 'app/core/auth/token-storage.service';
import { navigation } from 'app/navigation/navigation';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  navigation: any;
  isHidden  : boolean = true;


  constructor(
    private _fuseSplashScreenService: FuseSplashScreenService,
    private _fuseNavigationService: FuseNavigationService,
    private _authService: AuthenticationService,
    private _tokenStorage: TokenStorage 
  )  { 

        if  (environment.production) {
          
          this._authService.isAuthorized().subscribe((isAuthorized: any) => {
              
            if (isAuthorized) {

                var role = this._tokenStorage.getAccessData2().role;

                this.initNavigation();
                this.updateNavigationItem('administrator', true);

                if (role == undefined) {

                  if  (role == 'Superuser') {
                      this.isHidden = false;
                  }
                  this.updateNavigationItem('administrator', this.isHidden);
                    
                } else {
                    if  (role == 'Superuser') {
                        this.isHidden = false;
                    }
                    this.updateNavigationItem('administrator', this.isHidden);
                }

              }
              
          });

      } else {
          this.initNavigation();
      }

  }

  private updateNavigationItem(menuId: string, isHidden: boolean) {
    //hide audit administrator
    this._fuseNavigationService.updateNavigationItem(menuId, {
        hidden: isHidden
    })
}

private initNavigation() {

     // Get default navigation
     this.navigation = navigation;

     // Unregister
     this._fuseNavigationService.unregister('main')

     // Register the navigation to the service
     this._fuseNavigationService.register('main', this.navigation);

     // Set the main navigation as our current navigation
     this._fuseNavigationService.setCurrentNavigation('main');

}

}
