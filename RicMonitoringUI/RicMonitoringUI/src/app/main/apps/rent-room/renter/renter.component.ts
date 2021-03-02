import { Component, OnInit, ChangeDetectorRef, AfterViewInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { HttpEventType } from '@angular/common/http';
import { RenterDetailService } from './details/renter-details.service';
import { MatSnackBar } from '@angular/material';

@Component({
  selector: 'app-renter',
  templateUrl: './renter.component.html',
  styleUrls: ['./renter.component.scss'],
  animations: fuseAnimations,
})
export class RenterComponent implements OnInit {

  profileImage: string = 'assets/images/avatars/profile.jpg';
  pageType: string;
  renterName: string;
  renterId: number;

  public progress: number;
  public message: string;

  ngOnInit() {}

  constructor(
    private _activatedRoute: ActivatedRoute,
    private _renterDetailService: RenterDetailService,
    private _snackBar: MatSnackBar) 
  {

      this.renterId = Number(this._activatedRoute.snapshot.paramMap.get('id'))
      this.pageType = this._activatedRoute.snapshot.paramMap.get('id') == 'new' ? 'new' : 'edit';
    
      if (this.pageType == 'edit') {
        this.renterName = this._activatedRoute.snapshot.paramMap.get('handle').replace(/-/g, ' ').toUpperCase();
        setTimeout(()=> {
          this.profileImage = localStorage.getItem('base64');
        })
      }
        
  
  }

  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }
    let fileToUpload = <File>files[0];
    let reader = new FileReader();
    reader.readAsDataURL(fileToUpload);
      reader.onerror = function (error) {
      console.log('Error: ', error);
    };

    reader.onloadend = () => {
      this.postToServer(reader.result)
      this.profileImage = reader.result.toString();
    }
  }

  postToServer(base64) {
    var data = {
      renterId: this.renterId,
      base64
    }
    let message = "";
    this._renterDetailService.upload(data)
      .subscribe((response: any) => {
        if (response.payload == "UPLOAD_COMPLETED") {
            message = 'Image has been uploaded.'
        } else {
            message = 'error uploading file.'
        }

        this._snackBar.open(message, 'OK', {
          verticalPosition  : 'top',
          duration          : 2000
        });

      }, error => {
        
        console.log("error: ", error);

        this._snackBar.open("error uploading file. Please contact administrator", 'OK', {
          verticalPosition  : 'top',
          duration          : 2000
        });

      });
  }
  
}
