import { Component, OnInit } from '@angular/core';
import { FuseConfigService } from '@fuse/services/config.service';

@Component({
  selector: 'app-online-booking',
  templateUrl: './online-booking.component.html',
  styleUrls: ['./online-booking.component.scss']
})
export class OnlineBookingComponent implements OnInit {

  config: any;

  constructor(private _fuseConfigService: FuseConfigService) {

      this.setConfig();
  
  }

  setConfig(){
    // Configure the layout
    this._fuseConfigService.config = {
      layout: {
          navbar   : {
              hidden: true
          },
          toolbar  : {
              hidden: true
          },
          footer   : {
              hidden: true
          },
          sidepanel: {
              hidden: true
          }
      }
    };
  }

  ngOnInit() {
    // Subscribe to config change
    this._fuseConfigService.config
    .subscribe((config) => {
        this.config = config;
    });
  }

}
