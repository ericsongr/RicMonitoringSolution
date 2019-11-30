import { Component, OnInit, ViewChild, ElementRef, ViewEncapsulation } from '@angular/core';
import { MatPaginator, MatSort } from '@angular/material';
import { fuseAnimations } from '@fuse/animations';
import { Subject, fromEvent, BehaviorSubject, Observable, merge } from 'rxjs';
import { takeUntil, debounceTime, distinctUntilChanged, map } from 'rxjs/operators';
import { RentersService } from './renters.service';
import { FuseUtils } from '@fuse/utils';
import { DataSource } from '@angular/cdk/collections';

@Component({
  selector: 'page-renters',
  templateUrl: './renters.component.html',
  styleUrls: ['./renters.component.scss'],
  animations: fuseAnimations,
  encapsulation: ViewEncapsulation.None
})
export class RentersComponent implements OnInit {

  dataSource: FilesDataSource | null;
  displayedColumns = ['id','name','advancePaidDateString','startDateString', 'dueDateString', 'noOfPersons'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('filter') filter: ElementRef;
  @ViewChild(MatSort) sort: MatSort;
  
  private _unsubscribeAll = new Subject();
  constructor(
    private _rentersService: RentersService
  ) { }


  ngOnInit(): void {
    
    this.dataSource = new FilesDataSource(this._rentersService, this.paginator, this.sort);
    
    fromEvent(this.filter.nativeElement, 'keyup')
              .pipe(
                takeUntil(this._unsubscribeAll),
                debounceTime(150),
                distinctUntilChanged()
              )
              .subscribe(() => {

                if ( !this.dataSource) {
                  return;
                }
                this.dataSource.filter = this.filter.nativeElement.value;

              })
  }

  handleize(name) {
    return FuseUtils.handleize(name);
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
    private _rentersService: RentersService,
    private _paginator: MatPaginator,
    private _sort: MatSort)
  {

    super();

    this.filteredData = this._rentersService.renters;

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
      this._rentersService.onRentersChanged,
      this._paginator.page,
      this._filterChange,
      this._sort.sortChange
    ];

    return merge(...displayDataChanges).pipe(map(() => {
      
        let data = this._rentersService.renters.slice();

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
            [propertyA, propertyB] = [a.name, b.name];
            break;
        case 'advancePaidDate':
            [propertyA, propertyB] = [a.advancePaidDate, b.advancePaidDate];
            break;
        case 'startDate':
            [propertyA, propertyB] = [a.startDate, b.startDate];
            break;
        case 'dueDate':
            [propertyA, propertyB] = [a.dueDate, b.dueDate];
            break;
        case 'noOfPersons':
            [propertyA, propertyB] = [a.noOfPersons, b.noOfPersons];
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

