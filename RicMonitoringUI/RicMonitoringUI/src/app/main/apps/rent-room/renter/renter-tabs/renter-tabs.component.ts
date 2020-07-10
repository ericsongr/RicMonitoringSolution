import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router, NavigationStart, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'renter-tabs',
  templateUrl: './renter-tabs.component.html',
  styleUrls: ['./renter-tabs.component.scss']
})
export class RenterTabsComponent implements OnInit, AfterViewInit {

  navLinks: any[];
  activeLinkIndex = -1; 
  routerParams: any;
  id:any;

  constructor(private _router: Router
              ,private _activatedRoute: ActivatedRoute
              ) 
  { 
    this.id = this._activatedRoute.snapshot.paramMap.get('id');
    
    this.initNavLink();
  }



  ngOnInit() {
    
    this._router.events.subscribe((res: NavigationStart)=> {
      this.activeLinkIndex = this.navLinks.indexOf(this.navLinks.find(tab => tab.link === res.url));
    });

  }

  public ngAfterViewInit() {
  }

  initNavLink() {
      
    this.navLinks = [{
        label: 'Details',
        link: 'details',
        icon: 'library_books',
        index: 0
      }];

    if (this.id != 'new') {
        this.navLinks.push({
          label: 'History',
          link: 'payment-history',
          icon: 'history',
          index: 1
        });
    }

  }

}
