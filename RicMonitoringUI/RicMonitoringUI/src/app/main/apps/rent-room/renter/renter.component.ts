import { Component, OnInit, OnDestroy, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { Renter } from './renter.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs/Subscription';
import { RenterService } from './renter.service';
import { MatSnackBar, MatRadioChange } from '@angular/material';
import { FuseUtils } from '@fuse/utils';
import { Location } from '@angular/common';
import { RoomsService } from '../rooms/rooms.service';
import { AppFunctionsService } from '../../common/services/app-functions.service';
import * as moment from 'moment'
import { RentTransactionHistoryService } from '../rent-transaction-history/rent-transaction-history.service';
import { RentTransactionHistory } from '../rent-transaction-history/rent-transaction-history.model';
import { FuseProgressBarService } from '@fuse/components/progress-bar/progress-bar.service';

@Component({
  selector: 'app-renter',
  templateUrl: './renter.component.html', 
  styleUrls: ['./renter.component.scss']
})
export class RenterComponent implements OnInit, OnDestroy, AfterViewInit {
  
  renter = new Renter();
  pageType: string;
  renterForm: FormGroup;
  onRenterChanged: Subscription;
  onRentTransactionHistorySubscription: Subscription;

  rooms: any;
  daysWithSuffix: any;
  
  isShowBreakdown: boolean = false;
  monthlyRentPrice: number;
  hasBalance: boolean = true;
  dueDay: string;

  rentTransactionHistory: RentTransactionHistory[];
  
  constructor(
    private _appFunctionsService: AppFunctionsService,
    private _renterService: RenterService,
    private _rentTransactionHistoryService: RentTransactionHistoryService,
    private _roomsService: RoomsService,
    private _fuseProgressBarService: FuseProgressBarService,
    private _formBuilder: FormBuilder,
    private _snackBar : MatSnackBar,
    private _location : Location,
    private _cdr: ChangeDetectorRef
    ) { }
    
    ngOnInit() {
      
      this.onRenterChanged =
      this._renterService.onRenterChanged
          .subscribe(renter => {

            //fetch rooms
            this.fetchRooms();

            if(renter) {
              this.renter = new Renter(renter);
              this.pageType = 'edit';
              
              //delay after 1 second
              setTimeout(() => {
                this.selectRoom();
                this.onChangeTotalPaidAmount();
              });
              
              var dateStart = moment(this.renter.startDate).format('YYYY-MM-DD');
              //fetch days with suffix
              this.fetchDaysWithSuffix(dateStart);
              
              this.dueDay = renter.dueDay.toString();
              
            } 
            else 
            {
              this.pageType = 'add';
              this.renter = new Renter();

              var dateStart = moment().format('YYYY-MM-DD');
              //fetch days with suffix
              this.fetchDaysWithSuffix(dateStart);
              this.dueDay = moment().format('DD');

            }
            
            this.renterForm = this.createRenterForm();

          });

    }

    ngAfterViewInit(): void {
      
    }
    
    createRenterForm(): FormGroup {
      return this._formBuilder.group({
      id:               [this.renter.id],
      name:             [this.renter.name, Validators.required],
      advanceMonths:    [this.renter.advanceMonths, Validators.required],
      monthsUsed:       [this.renter.monthsUsed],
      advancePaidDate:  [this.renter.advancePaidDate, Validators.required],
      startDate:        [this.renter.startDate, Validators.required],
      dueDay:           [this.renter.dueDay, Validators.required],
      noOfPersons:      [this.renter.noOfPersons, Validators.required],
      roomId:           [this.renter.roomId, Validators.required],
      isEndRent:        [this.renter.isEndRent],
      dateEndRent:      [this.renter.dateEndRent],
      totalPaidAmount:  [this.renter.totalPaidAmount],
      balancePaidDate:  [this.renter.balancePaidDate],
    });
  }
  
  fetchRooms() {
    this._roomsService.getRooms("dropdown")
        .then(response => {
          this.rooms = response;
        })
  }

  fetchDaysWithSuffix(selectedDate: string) {
    //1st, 2nd, 6th
    this._appFunctionsService.getDaysWithSuffix(selectedDate)
        .then(response => {
          this.daysWithSuffix = response;
        })
        .catch((error) => {
          console.log(error);
        })
  }

  save() {
    const data = this.renterForm.getRawValue();
    data.balanceAmount = this.renter.balanceAmount;
    data.monthsUsed = data.monthsUsed == null ? 0 : data.monthsUsed;
    data.balancePaidDate = data.balancePaidDate = undefined ? null : data.balancePaidDate;
    data.handle = FuseUtils.handleize(data.name);
    data.startDateInput = moment(data.startDate).format('YYYY-MM-DD');
    data.advancePaidDateInput = moment(data.advancePaidDate).format('YYYY-MM-DD');
    
    if (!this.hasBalance){
      data.balancePaidDate = null;
      data.balanceAmount = 0;
    }

    this._renterService.saveRenter(data)
        .then(() => {

            //Trigger the subscription with new data
            // this._renterService.onRenterChanged.next(data);

            //show the success message
            this._snackBar.open('Renter detail saved.',
                'OK',
                {
                    verticalPosition: 'top',
                    duration: 2000
                });

            //change the location with new one
            this._location.go(`/apartment/tenants/${this.renter.id}/${this.renter.handle}`);
        });
  }

  add() {
    const data = this.renterForm.getRawValue();
    data.balanceAmount = this.renter.balanceAmount;
    data.monthsUsed = data.monthsUsed == null ? 0 : data.monthsUsed;
    data.balancePaidDate = data.balancePaidDate = undefined ? null : data.balancePaidDate;
    data.handle = FuseUtils.handleize(data.name);
    data.startDateInput = moment(data.startDate).format('YYYY-MM-DD');
    data.advancePaidDateInput = moment(data.advancePaidDate).format('YYYY-MM-DD');
    
    this._renterService.addRenter(data)
        .then((renterId) => {
          //Trigger the subscription with new data

          data.id = renterId;
          // this._renterService.onRenterChanged.next(data);

          //show the success message
          this._snackBar.open('New renter added.', 'OK', {
            verticalPosition  : 'top',
            duration          : 2000
          });

          //change the location with new one
          this._location.go(`/apartment/tenants/${this.renter.id}/${this.renter.handle}`);
          this._cdr.detectChanges();
        }).catch(error =>{
          console.log('error: ', error);
        });
  }

  selectRoom() {

    var advanceMonths = Number(this.renter.advanceMonths == undefined ? 0 : this.renter.advanceMonths);
 
    if(advanceMonths > 0) {
      var room = this.rooms.find(room => room.id == this.renter.roomId)
      if(room != undefined){
        this.monthlyRentPrice = Number(room.price);
        this.renter.totalAdvanceAmountDue = this.monthlyRentPrice * advanceMonths;
        this.isShowBreakdown = true;
      }
      else {
        this.isShowBreakdown = false;
      }
    }
   
    this.onChangeTotalPaidAmount();
  }

  get balancePaidDate() {
    return this.renterForm.get('balancePaidDate');
  }

  get dateEndRent() {
    return this.renterForm.get('dateEndRent');
  }


  onClickTabHistory() {
    
      this._fuseProgressBarService.show();
      
      this._rentTransactionHistoryService.getHistory(this.renter.id)
      
          .then((response: RentTransactionHistory[]) => {
            
            this.rentTransactionHistory = response;
            
            this._fuseProgressBarService.hide();

          })
    
  }

  onChangeTotalPaidAmount() {
      this.renter.balanceAmount = this.renter.totalAdvanceAmountDue - this.renter.totalPaidAmount;
      this.hasBalance = Number(this.renter.totalAdvanceAmountDue) > Number(this.renter.totalPaidAmount);
    
      if(!this.hasBalance){
        this.renter.balancePaidDate = null;
        this.balancePaidDate.setValidators(null);
      }
      else {
        this.balancePaidDate.setValidators([Validators.required])
      }
      this.balancePaidDate.updateValueAndValidity();
      this._cdr.detectChanges();

  }

  onChangeIsRentEnd(event: MatRadioChange) {
    debugger;
    this.renter.isEndRent = event.value == "true";
     if (this.renter.isEndRent) {
        this.dateEndRent.setValidators([Validators.required])
     }
     else {
      this.renter.dateEndRent = null;
      this.dateEndRent.setValidators(null);
     }
     this.dateEndRent.updateValueAndValidity();
     this._cdr.detectChanges();
  }

  ngOnDestroy(): void {
    this.onRenterChanged.unsubscribe();
  }
}
