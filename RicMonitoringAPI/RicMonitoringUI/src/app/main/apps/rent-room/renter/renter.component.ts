import { Component, OnInit, OnDestroy, AfterViewInit } from '@angular/core';
import { Renter } from './renter.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs/Subscription';
import { RenterService } from './renter.service';
import { MatSnackBar } from '@angular/material';
import { FuseUtils } from '@fuse/utils';
import { Location } from '@angular/common';
import { RoomsService } from '../rooms/rooms.service';

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
  rooms: any;
  
  isShowBreakdown: boolean = false;
  monthlyRentPrice: number;
  hasBalance: boolean = true;
  
  constructor(
    private _renterService: RenterService,
    private _roomsService: RoomsService,
    private _formBuilder: FormBuilder,
    private _snackBar : MatSnackBar,
    private _location : Location
    ) { }
    
    ngOnInit() {
      
      this.onRenterChanged =
      this._renterService.onRenterChanged
      .subscribe(renter => {
        if(renter) {
          this.renter = new Renter(renter);
          this.pageType = 'edit';

          //delay 1 second
          setTimeout(() => {
            this.onChangeTotalPaidAmount();
          });

        } 
        else 
        {
          this.pageType = 'add';
          this.renter = new Renter();
        }
        
        this.renterForm = this.createRenterForm();
        
        //fetch rooms
        this.fetchRooms();
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
      dueDate:          [this.renter.dueDate, Validators.required],
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

  save() {
    const data = this.renterForm.getRawValue();
    data.handle = FuseUtils.handleize(data.name);
    data.balanceAmount = this.renter.balanceAmount;
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
          this._location.go(`/apps/rent-room/renters/${this.renter.id}/${this.renter.handle}`);
        })
  }

  add() {
    const data = this.renterForm.getRawValue();
    data.balanceAmount = this.renter.balanceAmount;

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
          this._location.go(`/apps/rent-room/renters/${this.renter.id}/${this.renter.handle}`);
        })
  }

  selectRoom() {

    var advanceMonths = Number(this.renter.advanceMonths);
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
    
  }

  onChangeTotalPaidAmount() {
    this.selectRoom();
    this.hasBalance = Number(this.renter.totalAdvanceAmountDue) > Number(this.renter.totalPaidAmount);
    console.log('hasBalance:', this.hasBalance);
    this.renter.balanceAmount = this.renter.totalAdvanceAmountDue - this.renter.totalPaidAmount;
    
    if(!this.hasBalance){
      this.renter.balancePaidDate = null;
    }
  }
  ngOnDestroy(): void {
    this.onRenterChanged.unsubscribe();
  }
}
