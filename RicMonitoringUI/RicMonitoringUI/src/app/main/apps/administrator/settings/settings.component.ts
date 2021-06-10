import { Component, OnInit } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA, MatSnackBar } from '@angular/material';
import { Subscription } from 'rxjs';
import { EditSettingDialogComponent } from './edit-setting-dialog/edit-setting-dialog.component';
import { SettingsService } from './settings.service';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnInit {

  onSettingsSubscription: Subscription

  settings: any[]

  constructor(
    private _settingsService: SettingsService,
    private _snackBar: MatSnackBar,
    public _dialog: MatDialog
  ) { }

  ngOnInit() {

    this.onSettingsSubscription = 
      this._settingsService.onSettingsChanged
          .subscribe(settings => {
            this.settings = settings
          });

  }

  editSetting(key) {

    var setting = this.settings.find(p => p.key == key);
    console.log(key);
    const dialog = this._dialog.open(EditSettingDialogComponent, {
      data: setting,
      height: this.getHeight(key),
      width: '400px',
    });

    dialog.afterClosed().subscribe(response => {
      if (response !== 'cancel') {
        this.updateSetting(response.key, response.realValue)
      }
    })
  }

  onTicked(event, key) {
    this.updateSetting(key, event.value)    
  }

  updateSetting(key, value) {
    var data = {
      key,
      value
    }

    this._settingsService.save(data)
        .then((response: any) => {
          if(!response.errors.message) {
            //show the success message
            this._snackBar.open('Setting has been updated.', 'OK', {
              verticalPosition  : 'top',
              duration          : 2000
            });
        } else {
          this._snackBar.open('error occurred, updating setting.', 'OK', {
            verticalPosition  : 'top',
            duration          : 2000
          });
        }
      });
  }

 getHeight(key: string) {
   debugger;
  if(key == 'AppEmailMessageRenterBeforeDueDate') {
    return '300px';
  }else{
    return '200px';
  }
 }
 
 getWidth(key: string) {
  debugger;
  if(key == 'AppEmailMessageRenterBeforeDueDate') {
    return '400px';
  }else{
    return '100px';
  }
 }

}
