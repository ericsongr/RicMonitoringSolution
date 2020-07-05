import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router, NavigationStart } from '@angular/router';

@Component({
  selector: 'renter-tabs',
  templateUrl: './renter-tabs.component.html',
  styleUrls: ['./renter-tabs.component.scss']
})
export class RenterTabsComponent implements OnInit, AfterViewInit {

  navLinks: any[];
  activeLinkIndex = -1; 

  constructor(private _router: Router) { 
    this.initNavLink();
  }



  ngOnInit() {
    
    this._router.events.subscribe((res: NavigationStart)=> {
      this.activeLinkIndex = this.navLinks.indexOf(this.navLinks.find(tab => tab.link === res.url));
    });

  }

  public ngAfterViewInit() {

    // this._router.navigate([{ outlets: { 'tab': ['payment-history'] } }]);
  //   this._router.navigate([
  //     'details', {
  //         outlets: { tab: ['./details'] }
  //     }
  // ]);
  }

  initNavLink() {
      
    this.navLinks = [{
        label: 'Details',
        link: 'details',
        icon: 'library_books',
        index: 0
      },{
        label: 'History',
        link: 'payment-history',
        icon: 'history',
        index: 1
    }];

  }

}
