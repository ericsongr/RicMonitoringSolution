import { Injectable } from '@angular/core';

@Injectable()
export class ConfigService {
  private config: any;

  constructor() { 
    const window = this.getWindow();

    if (window['config']){
      this.config = window['config'];
      console.log('Configuration loaded', this.config);
    } else {
      console.log("No configuration found in window object.");
    }

  }

  getWindow() {
    return window;
  }

  get baseUrl() {
    return this.config.baseUrl;
  }

}
