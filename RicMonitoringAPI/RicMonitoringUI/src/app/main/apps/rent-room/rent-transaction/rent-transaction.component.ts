import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { RentTransaction } from './rent-transaction.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs/Subscription';
import { RentTransactionService } from './rent-transaction.service';
import { MatSnackBar } from '@angular/material';
import { FuseUtils } from '@fuse/utils';
import { Location } from '@angular/common';
import { RoomsService } from '../rooms/rooms.service';

@Component({
  selector: 'app-renter',
  templateUrl: './renter.component.html', 
  styleUrls: ['./renter.component.scss']
})
export class RentTransactionComponent implements OnInit, OnDestroy, AfterViewInit {
  
  rentTransaction = new RentTransaction();
  pageType: string;
  renterForm: FormGroup;
  onRenterChanged: Subscription;
  rooms: any;
  
  isShowBreakdown: boolean = false;
  monthlyRentPrice: number;
  hasBalance: boolean = true;
  
  constructor(
    private _renterService: RentTransactionService,
    private _roomsService: RoomsService,
    private _formBuilder: FormBuilder,
    private _snackBar : MatSnackBar,
    private _location : Location
    ) { }
    
    ngOnInit() {
      debugger;
      this.onRenterChanged =
      this._renterService.onRenterChanged
      .subscribe(renter => {
        if(renter) {
          this.rentTransaction = new RentTransaction(renter);
          this.pageType = 'edit';

          //delay 1 second
          setTimeout(() => {
            this.onChangeTotalPaidAmount();
          });

        } 
        else 
        {
          this.pageType = 'add';
          this.rentTransaction = new RentTransaction();
        }
        
        this.renterForm = this.createRentTransactionForm();
        
        //fetch rooms
        this.fetchRooms();
      });
    }

    ngAfterViewInit(): void {
      
    }
    
    createRentTransactionForm(): FormGroup {
      return this._formBuilder.group({
      id:               [this.rentTransaction.id],
      name:             [this.rentTransaction.name, Validators.required],
      advanceMonths:    [this.rentTransaction.advanceMonths, Validators.required],
      monthsUsed:       [this.rentTransaction.monthsUsed],
      advancePaidDate:  [this.rentTransaction.advancePaidDate, Validators.required],
      startDate:        [this.rentTransaction.startDate, Validators.required],
      dueDate:          [this.rentTransaction.dueDate, Validators.required],
      noOfPersons:      [this.rentTransaction.noOfPersons, Validators.required],
      roomId:           [this.rentTransaction.roomId, Validators.required],
      isEndRent:        [this.rentTransaction.isEndRent],
      dateEndRent:      [this.rentTransaction.dateEndRent],
      totalPaidAmount:  [this.rentTransaction.totalPaidAmount],
      balancePaidDate:  [this.rentTransaction.balancePaidDate],
    });
  }
  
  fetchRooms() {
    this._roomsService.getRooms("dropdown")
        .then(response => {
          this.rooms = response;
        })
  }

  save() {
    const data = this.renterForm.getRawValue();
    data.handle = FuseUtils.handleize(data.name);
    data.balanceAmount = this.rentTransaction.balanceAmount;
    console.log()
    console.log('data: ', data);
    this._renterService.saveRenter(data)
        .then(() => {

          //Trigger the subscription with new data
          this._renterService.onRenterChanged.next(data);

          //show the success message
          this._snackBar.open('Renter detail saved.', 'OK', {
            verticalPosition  : 'top',
            duration          : 2000
          });

          //change the location with new one
          this._location.go(`/apps/rent-room/renters/${this.rentTransaction.id}/${this.rentTransaction.handle}`);
        })
  }

  add() {
    const data = this.renterForm.getRawValue();
    data.balanceAmount = this.rentTransaction.balanceAmount;

    data.handle = FuseUtils.handleize(data.name);

    this._renterService.addRenter(data)
        .then(() => {

          //Trigger the subscription with new data
          this._renterService.onRenterChanged.next(data);

          //show the success message
          this._snackBar.open('New renter added.', 'OK', {
            verticalPosition  : 'top',
            duration          : 2000
          });

          //change the location with new one
          this._location.go(`/apps/rent-room/renters/${this.rentTransaction.id}/${this.rentTransaction.handle}`);
        })
  }

  selectRoom() {

    var advanceMonths = Number(this.rentTransaction.advanceMonths);
    if(advanceMonths > 0) {
      var room = this.rooms.find(room => room.id == this.rentTransaction.roomId)
      if(room != undefined){
        this.monthlyRentPrice = Number(room.price);
        this.rentTransaction.totalAdvanceAmountDue = this.monthlyRentPrice * advanceMonths;
        this.isShowBreakdown = true;
      }
      else {
        this.isShowBreakdown = false;
      }
    }
    
  }

  onChangeTotalPaidAmount() {
    this.selectRoom();
    this.hasBalance = Number(this.rentTransaction.totalAdvanceAmountDue) > Number(this.rentTransaction.totalPaidAmount);
    console.log('hasBalance:', this.hasBalance);
    this.rentTransaction.balanceAmount = this.rentTransaction.totalAdvanceAmountDue - this.rentTransaction.totalPaidAmount;
    
    if(!this.hasBalance){
      this.rentTransaction.balancePaidDate = null;
    }
  }
  ngOnDestroy(): void {
    this.onRenterChanged.unsubscribe();
  }
}
