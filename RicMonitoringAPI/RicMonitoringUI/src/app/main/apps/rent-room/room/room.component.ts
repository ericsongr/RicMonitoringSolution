import { Component, OnInit, OnDestroy } from '@angular/core';
import { Room } from './room.model';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs/Subscription';
import { RoomService } from './room.service';
import { MatSnackBar } from '@angular/material';
import { FuseUtils } from '@fuse/utils';
import { Location } from '@angular/common';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss']
})
export class RoomComponent implements OnInit, OnDestroy {
  
  room = new Room();
  pageType: string;
  roomForm: FormGroup;
  onRoomChanged: Subscription;
  
  constructor(
    private _roomService: RoomService,
    private _formBuilder: FormBuilder,
    private _snackBar : MatSnackBar,
    private _location : Location
    ) { }
    
    ngOnInit() {
      this.onRoomChanged =
        this._roomService.onRoomChanged
            .subscribe(room => {
              if(room) {
                this.room = new Room(room);
                this.pageType = 'edit';
              } 
              else 
              {
                this.pageType = 'add';
                this.room = new Room();
                
              }

              this.roomForm = this.createRoomForm();
            });
    }

  createRoomForm(): FormGroup {
    return this._formBuilder.group({
      id:         [this.room.id],
      name:       [this.room.name, Validators.required],
      frequency:  [this.room.frequency, Validators.required],
      price:      [this.room.price, Validators.required],
    });
  }
  
  save() {
    if (this.roomForm.invalid) {
      
      //show the success message
      this._snackBar.open('Invalid data. Please verify.', 'OK', {
        verticalPosition  : 'top',
        duration          : 2000,
        panelClass        : ['mat-warn']
      });

    } else {
      const data = this.roomForm.getRawValue();
      data.handle = FuseUtils.handleize(data.name);

      this._roomService.saveRoom(data)
          .then(() => {

            //Trigger the subscription with new data
            this._roomService.onRoomChanged.next(data);

            //show the success message
            this._snackBar.open('Room detail saved.', 'OK', {
              verticalPosition  : 'top',
              duration          : 2000
            });

            //change the location with new one
            this._location.go(`/apps/rent-room/rooms/${this.room.id}/${this.room.handle}`);
          });
      }
  }

  add() {

    if (this.roomForm.invalid) {
      
      //show the success message
      this._snackBar.open('Invalid data. Please verify.', 'OK', {
        verticalPosition  : 'top',
        duration          : 2000,
        panelClass        : ['mat-warn']
      });

    } else {

      const data = this.roomForm.getRawValue();
      data.handle = FuseUtils.handleize(data.name);
  
      this._roomService.addRoom(data)
          .then(() => {
  
            //Trigger the subscription with new data
            this._roomService.onRoomChanged.next(data);
  
            //show the success message
            this._snackBar.open('New room added.', 'OK', {
              verticalPosition  : 'top',
              duration          : 2000
            });
  
            //change the location with new one
            this._location.go(`/apps/rent-room/rooms/${this.room.id}/${this.room.handle}`);
          });
    }
    
  }

  

  ngOnDestroy(): void {
    this.onRoomChanged.unsubscribe();
  }
}
