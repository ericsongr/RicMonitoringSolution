import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { fuseAnimations } from '@fuse/animations';
import { Subject, BehaviorSubject, Observable, merge } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuditRoomsService } from './audit-rooms.service';
import { FuseUtils } from '@fuse/utils';
import { DataSource } from '@angular/cdk/collections';

@Component({
  selector: 'app-audit-rooms',
  templateUrl: './audit-rooms.component.html',
  styleUrls: ['./audit-rooms.component.scss'],
  animations: fuseAnimations,
  encapsulation: ViewEncapsulation.None
})
export class AuditRoomsComponent implements OnInit {
  debugger;
  dataSource: FilesDataSource | null;
  displayedColumns = ['auditRoomId','id','name','frequency','price','auditDateTimeString','username','auditAction'];
  
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('filter') filter: ElementRef;
  @ViewChild(MatSort) sort: MatSort;
  
  private _unsubscribeAll = new Subject();
  constructor(
    private _AuditRoomsService: AuditRoomsService
  ) { }
  
  
    ngOnInit(): void {
      
      this.dataSource = new FilesDataSource(this._AuditRoomsService, this.paginator, this.sort);
      
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
    private _auditRoomsService: AuditRoomsService,
    private _paginator: MatPaginator,
    private _sort: MatSort)
  {
  
    super();
  
    this.filteredData = this._auditRoomsService.auditRooms;
  
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
      this._auditRoomsService.onAuditRoomsChanged,
      this._paginator.page,
      this._filterChange,
      this._sort.sortChange
    ];
  
    return merge(...displayDataChanges).pipe(map(() => {
      
        let data = this._auditRoomsService.auditRooms.slice();
  
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
        case 'id':
            [propertyA, propertyB] = [a.id, b.id];
            break;
        case 'name':
            [propertyA, propertyB] = [a.phoneNumber, b.phoneNumber];
            break;
        case 'frequency':
            [propertyA, propertyB] = [a.email, b.email];
            break;
        case 'price':
            [propertyA, propertyB] = [a.isActive, b.isActive];
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