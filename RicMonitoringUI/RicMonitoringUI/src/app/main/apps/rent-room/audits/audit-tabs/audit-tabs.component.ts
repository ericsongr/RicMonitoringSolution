import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router, NavigationStart, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'audit-tabs',
  templateUrl: './audit-tabs.component.html',
  styleUrls: ['./audit-tabs.component.scss']
})
export class AuditTabsComponent implements OnInit, AfterViewInit {

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
        label: 'Rooms',
        link: 'rooms',
        icon: 'library_books',
        index: 0
      },{
        label: 'Renters',
        link: 'renters',
        icon: 'library_books',
        index: 1
      },{
        label: 'Rent Transactions',
        link: 'rent-transactions',
        icon: 'library_books',
        index: 2
      }];

  }

}
