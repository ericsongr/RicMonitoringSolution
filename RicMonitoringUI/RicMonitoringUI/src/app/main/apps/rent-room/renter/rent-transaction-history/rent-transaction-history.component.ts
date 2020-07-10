import { RentTransactionHistoryService } from './rent-transaction-history.service';
import { RentTransactionHistory } from './rent-transaction-history.model';

import { Component, OnInit, ViewChild, ElementRef, OnDestroy } from '@angular/core';
import { Subscription, Observable, Subject, merge, BehaviorSubject } from 'rxjs';
import { takeUntil, debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { DataSource } from '@angular/cdk/table';
import { MatPaginator, MatSort } from '@angular/material';
import { FuseUtils } from '@fuse/utils';
import { fuseAnimations } from '@fuse/animations';

@Component({
  selector: 'tab-rent-transaction-history',
  templateUrl: './rent-transaction-history.component.html',
  animations: fuseAnimations,
  styleUrls: ['./rent-transaction-history.component.scss'],
})
export class RentTransactionHistoryComponent implements OnInit, OnDestroy  {

  dataSource: FilesDataSource | null;
  displayedColumns = ['id','period','dueDateString','monthlyRent','previousBalance','currentBalance','totalAmountDue','paidAmount','isDepositUsed','paidOrUsedDepositDateString','balanceDateToBePaidString','note','transactionType']
  
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('filter') filter: ElementRef;
  @ViewChild(MatSort) sort: MatSort;

  onRentTransactionHistorySubscription: Subscription

  private _unsubscribeAll = new Subject();
  constructor(
    private _rentTransactionHistoryService: RentTransactionHistoryService
  ) { }

  ngOnInit(): void {
    this.dataSource = new FilesDataSource(this._rentTransactionHistoryService, this.paginator, this.sort);
  }

  ngOnDestroy():void
  {
    //unsubscribe from all subscriptions
    this._unsubscribeAll.next();
    this._unsubscribeAll.complete();

  }
  
}

export class FilesDataSource extends DataSource<any> 
{
  // private
  private _filterChange = new BehaviorSubject('');
  private _filteredDataChange = new BehaviorSubject('');

  constructor(
    private _rentTransactionHistoryService: RentTransactionHistoryService,
    private _paginator: MatPaginator,
    private _sort: MatSort)
  {

    super();

    this.filteredData = this._rentTransactionHistoryService.rentTransactionHistories;
  }

  
  // -----------------------------------------------------------------------------------------------------
  // @ Accessors
  // -----------------------------------------------------------------------------------------------------

  get filteredData(): any{
      return this._filteredDataChange.value;
  }

  set filteredData(value: any)
  {
    this._filteredDataChange.next(value);
  }

  get filter() : string 
  {
    return this._filterChange.value;
  }

  set filter(filter: string)
  {
    this._filterChange.next(filter);
  }


      // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Connect function called by the table to retrieve one stream containing the data to render.
     *
     * @returns {Observable<any[]>}
     */

  connect(): Observable<any[]>
  {
    const displayDataChanges = [
      this._rentTransactionHistoryService.onRentTransactionHistoryChanged,
      this._paginator.page,
      this._filterChange,
      this._sort.sortChange
    ];

    return merge(...displayDataChanges).pipe(map(() => {
      
        let data = this._rentTransactionHistoryService.rentTransactionHistories.slice();

        data = this.filterData(data);
        
        this.filteredData = [...data];

        data = this.sortData(data);
        
        // Grab the page's slice of data.
        const startIndex = this._paginator.pageIndex * this._paginator.pageSize;

        return data.splice(startIndex, this._paginator.pageSize);
      })
    );
  }

  /**
   * Filter data
   *
   * @param data
   * @returns {any}
   */
  
  filterData(data){
    if (!this.filter) {
      return data;
    }

    return FuseUtils.filterArrayByString(data, this.filter);
  }

    /**
     * Sort data
     *
     * @param data
     * @returns {any[]}
     */
  sortData(data): any[]
  {

    if ( !this._sort.active || this._sort.direction === ''){
      return data;
    }

    return data.sort((a, b) =>{
      let propertyA: number | string = '';
      let propertyB: number | string = '';

      switch ( this._sort.active) {
        case 'totalAmountDue':
            [propertyA, propertyB] = [a.totalAmountDue, b.totalAmountDue];
            break;
        case 'dueDate':
            [propertyA, propertyB] = [a.dueDate, b.dueDate];
            break;
        case 'transactionType':
            [propertyA, propertyB] = [a.transactionType, b.transactionType];
            break;
      }

      const valueA = isNaN(+propertyA) ? propertyA : +propertyA;
      const valueB = isNaN(+propertyB) ? propertyB : +propertyB;

      return (valueA < valueB ? -1: 1) * (this._sort.direction === 'asc' ? 1 : -1);
    });

  }

  disconnect(): void
  {}
}