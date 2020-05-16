import { Component, Inject} from '@angular/core';
import { FuseConfigService } from '@fuse/services/config.service';
import { FuseSplashScreenService } from '@fuse/services/splash-screen.service';
import { AuthService } from '../apps/common/core/auth/auth.service';
import { ApiControllers } from 'environments/api-controllers';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  constructor(
    private _authService: AuthService,
    private _fuseSplashScreenService: FuseSplashScreenService,
    @Inject("API_URL") private _apiUrl: string
  ) 
  { 

    //execute daily batch apartment transaction process
    this.execStoreProc();

  }

  execStoreProc() {

    setTimeout(() => {

      if (localStorage.getItem('isCallback') == 'true' && this._authService.isAuthorized) {

        localStorage.setItem('isCallback', 'false');
        this._fuseSplashScreenService.show();
      
        var url = `${this._apiUrl}${ApiControllers.ExecStoreProc}`;
        this._authService.post(url,{})
            .subscribe((dailyBatch) => { 
              if (dailyBatch.status == "Processing" || dailyBatch.status == "Processed"){
                this._fuseSplashScreenService.hide();
                localStorage.setItem('isCallback', 'false');
              }
            })
        }

    }, 1000);

  }

}
