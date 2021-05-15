import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { FuseConfigService } from '@fuse/services/config.service';
import { fuseAnimations } from '@fuse/animations';
import { AuthenticationService } from 'app/core/auth/authentication.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material';
import { FuseSplashScreenService } from '@fuse/services/splash-screen.service';
import { AccountsService } from '../../administrator/accounts/accounts.service';

@Component({
    selector     : 'login',
    templateUrl  : './login.component.html',
    styleUrls    : ['./login.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations   : fuseAnimations
})
export class LoginComponent implements OnInit
{
    loginForm: FormGroup;

    /**
     * Constructor
     *
     * @param {FuseConfigService} _fuseConfigService
     * @param {FormBuilder} _formBuilder
     */
    constructor(
        private _fuseSplashScreenService: FuseSplashScreenService,
        private _fuseConfigService: FuseConfigService,
        private _formBuilder: FormBuilder,
        private _authService: AuthenticationService,
        private _accountsService: AccountsService,
        private _router: Router,
        private _snackBar: MatSnackBar

    )
    {
        // Configure the layout
        this._fuseConfigService.config = {
            layout: {
                navbar   : {
                    hidden: true
                },
                toolbar  : {
                    hidden: true
                },
                footer   : {
                    hidden: true
                },
                sidepanel: {
                    hidden: true
                }
            }
        };
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void
    {
        this.loginForm = this._formBuilder.group({
            userName    : ['ericson', [Validators.required]],
            password    : ['Terno)48', Validators.required]
        });

        setTimeout(() => this.login(),2000);
    }

    login() {
        const data = this.loginForm.getRawValue();
        data.deviceId = "9267EDAE-2A8C-4EE2-AF49-84F57171F552";
        data.platform = "Web";
        
        this._fuseSplashScreenService.show()

        this._authService.login(data)
            .subscribe(response => {

                this._accountsService.setAccountId("10");

                this._fuseSplashScreenService.hide();

                if (!response.errors.message) { 
                    this._router.navigate(['/']);
                }
                else {
                    //show the success message
                    this._snackBar.open(response.errors.message, 'OK', {
                        verticalPosition  : 'top',
                        duration          : 2000
                    });
                }
                
            })
    }
}
