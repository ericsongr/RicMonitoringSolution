import { Component, OnInit, AfterViewInit, TemplateRef, ViewChild, ChangeDetectionStrategy } from '@angular/core';
import { DialogDeletePaymentConfirmationComponent } from 'app/main/apps/rent-room/rent-transaction/dialog-delete-payment-confirmation/dialog-delete-payment-confirmation.component';
import { MatDialog } from '@angular/material';
import { Router } from '@angular/router';

@Component({
  selector: 'change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ChangePasswordComponent implements AfterViewInit {
  @ViewChild('dialog') template: TemplateRef<any>
  
  constructor(
    private dialog: MatDialog,
    private router: Router
  ) { }

  ngAfterViewInit() {
    
    const ref = this.dialog.open(this.template, {
      width: '350px'
    });

    ref.afterClosed().subscribe(() => {
      this.router.navigate(['']);
    })
  }

}
