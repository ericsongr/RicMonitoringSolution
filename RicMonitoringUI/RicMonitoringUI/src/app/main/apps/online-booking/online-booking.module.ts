import { NgModule } from "@angular/core";
import { FuseSharedModule } from "@fuse/shared.module";
import { RouterModule, Routes } from "@angular/router";
import { FuseWidgetModule, FuseDemoModule } from "@fuse/components";
import { MaterialModule } from "app/main/module/material.module";
import { MatSelectCountryModule } from '@angular-material-extensions/select-country';
import { OnlineBookingComponent } from "./online-booking/online-booking.component";
import { OnlineBookingService } from "./online-booking/online-booking.service";
import { LookupTypeItemsService } from "../common/services/lookup-type-items.service";
import { OnlineBookingShowErrorsComponent } from "../common/online-booking-show-errors.component";

const routes : Routes = [{
    path        : '',
    component   : OnlineBookingComponent,
    resolve     : {
            data: OnlineBookingService
    }
}]

@NgModule({
    imports: [
        FuseSharedModule,
        RouterModule.forChild(routes),
        FuseWidgetModule,
        MaterialModule,
        FuseDemoModule,
        MatSelectCountryModule
    ],
    declarations: [
        OnlineBookingComponent,
        OnlineBookingShowErrorsComponent
    ],
    providers: [
        OnlineBookingService,
        LookupTypeItemsService
    ]
})
export class OnlineBookingModule {}