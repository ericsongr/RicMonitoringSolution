import { NgModule } from "@angular/core";
import { FuseSharedModule } from "@fuse/shared.module";
import { RouterModule, Routes } from "@angular/router";
import { FuseWidgetModule, FuseDemoModule } from "@fuse/components";
import { MaterialModule } from "app/main/module/material.module";
import { MatSelectCountryModule } from '@angular-material-extensions/select-country';
import { ShowErrorsComponent } from "../common/show-errors.component";
import { OnlineBookingComponent } from "./online-booking/online-booking.component";
import { OnlineBookingService } from "./online-booking/online-booking.service";

const routes : Routes = [{
    path        : '',
    component   : OnlineBookingComponent,
    // resolve     : {
    //         data: OnlineBookingService
    // }
}

]

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
        ShowErrorsComponent,
        OnlineBookingComponent
    ],
    providers: [
        OnlineBookingService
    ]
})
export class OnlineBookingModule {}