// import { Component, OnInit, OnDestroy } from '@angular/core';
// import { FormGroup, FormBuilder, Validators } from '@angular/forms';
// import { Subscription } from 'rxjs/Subscription';
// import { MatSnackBar } from '@angular/material';
// import { FuseUtils } from '@fuse/utils';
// import { Location } from '@angular/common';
// import { fuseAnimations } from '@fuse/animations';

// import { UserService } from './user.service';

// import { User } from './user.model';
// import { Role } from './role.model';
// import { Router } from '@angular/router';


// @Component({
//   selector: 'app-user',
//   templateUrl: './user.component.html',
//   styleUrls: ['./user.component.scss'],
//   animations: [fuseAnimations]
// })
// export class UserComponent implements OnInit, OnDestroy {
  
//   user = new User();
//   roles: Role[];
//   pageType: string;
  
//   userEntryForm: FormGroup;

//   onUserChangedSubscription: Subscription;
  
//   constructor(
//     private _userService: UserService,
//     private _formBuilder: FormBuilder,
//     private _snackBar : MatSnackBar,
//     private _location : Location,
//     private _router: Router
//     ) { }
    
//     ngOnInit() {
      
//       this.onUserChangedSubscription =
//         this._userService.onUserChanged
//             .subscribe(response => {
      
//               if(response.payload) {
//                 this.user = new User(response.payload);
//                 this.pageType = 'edit';
//               } 
//               else 
//               {
//                 this.pageType = 'add';
//                 this.user = new User();
//                 // this.user.lastName = 'gonzaga',
//                 // this.user.firstName = 'eddie',
//                 // this.user.email = 'eddie@yahoo.com',
//                 // this.user.mobileNumber = '09999999999',
//                 // this.user.phoneNumber = '09999999999',
//                 // this.user.role = 'Administrator',
//                 // this.user.userName = 'eddie',
//               }
              
//               //get all roles
//               this.getRoles();
              
//               this.userEntryForm = this.createUserForm();
              
//             });

            
       
           
//     }

//   getRoles() {
//     this._userService.getRoles()
//         .then((response: Role[]) => {
//           this.roles = response;
//         })
//   }

//   createUserForm(): FormGroup {
//     return this._formBuilder.group({
//       id            : [this.user.id],
//       firstName     : [this.user.firstName, Validators.required],
//       lastName      : [this.user.lastName, Validators.required],
//       email         : [this.user.email, Validators.required],
//       mobileNumber  : [this.user.mobileNumber, Validators.required],
//       phoneNumber   : [this.user.phoneNumber],
//       userName      : [this.user.userName, Validators.required],
//       role          : [this.user.role]
//     });
//   }
  
//   save() {

//     if (this.userEntryForm.invalid) {

//       //show the success message
//       this._snackBar.open('Invalid data. Please verify.', 'OK', {
//         verticalPosition: 'top',
//         panelClass: ['mat-warn']
//       });

//     } else {
      
//       var formData = this.userEntryForm.getRawValue();
//       formData.handle = FuseUtils.handleize(formData.lastName);

//       this._userService.saveUser(formData)
//         .then(() => {

//           //Trigger the subscription with new data
//           this._userService.onUserChanged.next(formData);

//           //show the success message
//           this._snackBar.open('User detail saved.', 'OK', {
//             verticalPosition: 'top',
//             duration: 2000
//           });
//           debugger;
//           //change the location with new one
//           this._router.navigate([`/administrator/users`]);
//           // this._location.go(`/administrator/users`);
//         });
//     } 
//   }

//   add() {
//     if (this.userEntryForm.invalid) {
//       //show the success message
//       this._snackBar.open('Invalid data. Please verify.', 'OK', {
//         verticalPosition  : 'top',
//         duration          : 2000,
//         panelClass        : ['mat-warn']
//       });

//     } else {

//       var formData = this.userEntryForm.getRawValue();

//       formData.handle = FuseUtils.handleize(formData.lastName);
  
//       this._userService.addUser(formData)
//           .then(() => {
  
//             //Trigger the subscription with new data
//             this._userService.onUserChanged.next(formData);
  
//             //show the success message
//             this._snackBar.open('New user added.', 'OK', {
//               verticalPosition  : 'top',
//               duration          : 2000
//             });
  
//             //change the location with new one
//             this._location.go(`/administrator/users/${this.user.userName}/${this.user.handle}`);
//           });
//     }
    
//   }

  

//   ngOnDestroy(): void {
//     this.onUserChangedSubscription.unsubscribe();
//   }
// }
