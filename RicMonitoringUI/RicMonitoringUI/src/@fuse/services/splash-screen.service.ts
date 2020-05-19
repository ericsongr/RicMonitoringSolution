import { Inject, Injectable } from '@angular/core';
import { DOCUMENT } from '@angular/common';
import { animate, AnimationBuilder, AnimationPlayer, style } from '@angular/animations';
import { NavigationEnd, Router } from '@angular/router';

import { filter, take } from 'rxjs/operators';
import { AuthService } from 'app/main/apps/common/core/auth/auth.service';
import { ApiControllers } from 'environments/api-controllers';

@Injectable({
    providedIn: 'root'
})
export class FuseSplashScreenService
{
    splashScreenEl: any;
    player: AnimationPlayer;

    /**
     * Constructor
     *
     * @param {AnimationBuilder} _animationBuilder
     * @param _document
     * @param {Router} _router
     */
    constructor(
        private _animationBuilder: AnimationBuilder,
        @Inject(DOCUMENT) private _document: any,
        @Inject("API_URL") private _apiUrl: string,
        private _router: Router,
        private _authService : AuthService
        
    )
    {
        // Initialize
        this._init();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Initialize
     *
     * @private
     */
    private _init(): void
    {
        // Get the splash screen element
        this.splashScreenEl = this._document.body.querySelector('#fuse-splash-screen');

        // this._document.body.querySelector('#screen-message').innerText('ericson gonzaaga ramos');
        
        // If the splash screen element exists...
        if ( this.splashScreenEl )
        {
            this._router.events
                    .pipe(
                        filter((event => event instanceof NavigationEnd)),
                        take(1)
                    )
                    .subscribe(() => {
                        
                        setTimeout(() => {
                            
                            if (localStorage.getItem('isCallback') == 'true' && this._authService.isAuthorized) {
                                //execute daily batch apartment transaction process
                                this.execStoreProc();
                            } else {
                                // Hide it on the first NavigationEnd event
                                this.hide();
                                
                            }
                           
                        }, 1000);

                    });
        }
    }

    execStoreProc() {
        localStorage.setItem('isCallback', 'false');
        
        var url = `${this._apiUrl}${ApiControllers.ExecStoreProc}`;
        this._authService.post(url,{})
            .subscribe((dailyBatch) => { 
                if (dailyBatch.status == "Processing" || dailyBatch.status == "Processed"){
                this.hide();
                localStorage.setItem('isCallback', 'false');
                }
            });
      }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Show the splash screen
     */
    show(): void
    {
        this.player =
            this._animationBuilder
                .build([
                    style({
                        opacity: '0',
                        zIndex : '99999'
                    }),
                    animate('400ms ease', style({opacity: '1'}))
                ]).create(this.splashScreenEl);

        setTimeout(() => {
            this.player.play();
        }, 0);
    }

    /**
     * Hide the splash screen
     */
    hide(): void
    {
        this.player =
            this._animationBuilder
                .build([
                    style({opacity: '1'}),
                    animate('400ms ease', style({
                        opacity: '0',
                        zIndex : '-10'
                    }))
                ]).create(this.splashScreenEl);

        setTimeout(() => {
            this.player.play();
        }, 0);
    }
}
