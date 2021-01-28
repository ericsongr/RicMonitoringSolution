import { Component, Inject, OnInit} from '@angular/core';
import { FuseConfigService } from '@fuse/services/config.service';
import { FuseSplashScreenService } from '@fuse/services/splash-screen.service';
import { ApiControllers } from 'environments/api-controllers';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent {

  constructor(
    private _fuseSplashScreenService: FuseSplashScreenService,
    @Inject("API_URL") private _apiUrl: string
  )  { }

}
