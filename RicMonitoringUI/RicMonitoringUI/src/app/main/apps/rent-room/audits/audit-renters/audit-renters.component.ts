
import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { fuseAnimations } from '@fuse/animations';
import { Subject, BehaviorSubject, Observable, merge } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuditRentersService } from './audit-renters.service';
import { FuseUtils } from '@fuse/utils';
import { DataSource } from '@angular/cdk/collections';

@Component({
  selector: 'audit-renters',
  templateUrl: './audit-renters.component.html',
  styleUrls: ['./audit-renters.component.scss'],
  animations: fuseAnimations,
  encapsulation: ViewEncapsulation.None
})
export class AuditRentersComponent implements OnInit {
  debugger;
  dataSource: FilesDataSource | null;
  displayedColumns = ['id','name','advancePaidDateString','startDateString','dueDayString','noOfPersons','roomName','advanceMonths','monthsUsed','dateEndString','isEndRent','balanceAmount','balancePaidDateString','totalPaidAmount','nextDueDateString','previousDueDateString','auditDateTimeString','username','auditAction'];
  
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('filter') filter: ElementRef;
  @ViewChild(MatSort) sort: MatSort;
  
  private _unsubscribeAll = new Subject();
  constructor(
    private _AuditRentersService: AuditRentersService
  ) { }
  
  
    ngOnInit(): void {
      
      this.dataSource = new FilesDataSource(this._AuditRentersService, this.paginator, this.sort);
      
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
    private _auditRentersService: AuditRentersService,
    private _paginator: MatPaginator,
    private _sort: MatSort)
  {
  
    super();
  
    this.filteredData = this._auditRentersService.auditRenters;
  
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
      this._auditRentersService.onAuditRentersChanged,
      this._paginator.page,
      this._filterChange,
      this._sort.sortChange
    ];
  
    return merge(...displayDataChanges).pipe(map(() => {
      
        let data = this._auditRentersService.auditRenters.slice();
  
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
        case 'auditRentTransactionId':
            [propertyA, propertyB] = [a.auditRentTransactionId, b.auditRentTransactionId];
            break;
        case 'id':
            [propertyA, propertyB] = [a.id, b.id];
            break;
        case 'paidDateString':
            [propertyA, propertyB] = [a.paidDateString, b.paidDateString];
            break;
        case 'dueDateString':
            [propertyA, propertyB] = [a.dueDateString, b.dueDateString];
            break;
        case 'period':
          [propertyA, propertyB] = [a.period, b.period];
          break;
        case 'auditDateTimeString':
          [propertyA, propertyB] = [a.auditDateTimeString, b.auditDateTimeString];
          break;
        case 'username':
          [propertyA, propertyB] = [a.username, b.username];
          break;
        case 'auditAction':
          [propertyA, propertyB] = [a.auditAction, b.auditAction];
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
