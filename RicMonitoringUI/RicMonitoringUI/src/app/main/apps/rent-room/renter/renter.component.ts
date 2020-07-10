import { Component, OnInit, ChangeDetectorRef, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';

@Component({
  selector: 'app-renter',
  templateUrl: './renter.component.html',
  styleUrls: ['./renter.component.scss'],
  animations: fuseAnimations,
})
export class RenterComponent implements OnInit {

  pageType: string;
  renterName: string;

  ngOnInit() {}

  constructor(private _activatedRoute: ActivatedRoute) {
    this.pageType = this._activatedRoute.snapshot.paramMap.get('id') == 'new' ? 'new' : 'edit';
    
    if (this.pageType == 'edit')
      this.renterName = this._activatedRoute.snapshot.paramMap.get('handle').replace('-', ' ').toUpperCase();
  
  }
  
}
