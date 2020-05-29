import { Component, OnInit, Input, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'tab-rent-transaction-history',
  templateUrl: './rent-transaction-history.component.html',
  styleUrls: ['./rent-transaction-history.component.scss']
})
export class RentTransactionHistoryComponent implements OnChanges {

  @Input('history') history;
  constructor() { 
    
  }
  ngOnChanges(changes: SimpleChanges): void {

    this.history = changes.history.currentValue;

  }

 
  
}
