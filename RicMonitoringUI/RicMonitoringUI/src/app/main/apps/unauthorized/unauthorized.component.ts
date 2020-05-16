import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Location } from '@angular/common';
import { AuthService } from '../common/core/auth/auth.service';
import { fuseAnimations } from '@fuse/animations';
import { FuseConfigService } from '@fuse/services/config.service';

@Component({
    selector: 'unauthorized',
    templateUrl: './unauthorized.component.html',
    styleUrls: ['./unauthorized.component.scss'],
    encapsulation: ViewEncapsulation.None,
    animations: fuseAnimations
})
export class UnauthorizedComponent implements OnInit {

    constructor(
        private _location: Location,
        private _fuseConfigService: FuseConfigService, 
        private _authService: AuthService) 
    {
        this._fuseConfigService.config = {
            layout: {
                navbar: {
                    hidden: true
                },
                toolbar: {
                    hidden: true,
                },
                footer: {
                    hidden: true,
                },
                sidepanel: {
                    hidden: true
                }
            }
        }
    }

    ngOnInit() {
    }

    login() {
        this._authService.login();
    }

    goback() {
        this._location.back();
    }
}