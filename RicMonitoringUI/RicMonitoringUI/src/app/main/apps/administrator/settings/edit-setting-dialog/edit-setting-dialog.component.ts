import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

@Component({
  selector: 'app-edit-setting-dialog',
  templateUrl: './edit-setting-dialog.component.html',
  styleUrls: ['./edit-setting-dialog.component.scss']
})
export class EditSettingDialogComponent {

  setting: any;
  useTextArea: boolean = false;

  constructor(
    public dialogRef: MatDialogRef<EditSettingDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) 
    { 
      this.setting = data;

      this.useTextArea = this.setting.key == 'AppEmailMessageRenterBeforeDueDate';
    }

  cancel() {
    this.dialogRef.close('cancel');
  }

}
