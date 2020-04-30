import { Component, OnDestroy } from '@angular/core';

import { FuseTranslationLoaderService } from '@fuse/services/translation-loader.service';

import { locale as english } from './i18n/en';
import { locale as turkish } from './i18n/tr';
import { AuthService } from '../apps/common/core/auth/auth.service';
import { Subscription } from 'rxjs';

@Component({
    selector   : 'sample',
    templateUrl: './sample.component.html',
    styleUrls  : ['./sample.component.scss']
})
export class SampleComponent implements OnDestroy
{
    private isAuthorizedSubscription: Subscription = new Subscription();
    public isAuthorized = false;
    /**
     * Constructor
     *
     * @param {FuseTranslationLoaderService} _fuseTranslationLoaderService
     */
    constructor(
        private _fuseTranslationLoaderService: FuseTranslationLoaderService,
        public _authService: AuthService
    )
    {

        this._fuseTranslationLoaderService.loadTranslations(english, turkish);

         this.isAuthorizedSubscription = this._authService.getIsAuthorized()
         .subscribe((isAuthorized: boolean) => {
            this.isAuthorized = isAuthorized;
         });
    }

    login() {
        this._authService.login();
    }

    logout() {
        this._authService.logout();
    }

    ngOnDestroy() : void {
        this.isAuthorizedSubscription.unsubscribe();
    }
}
