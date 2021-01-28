import { Component, Input } from "@angular/core";
import { AbstractControlDirective, AbstractControl } from "@angular/forms";

@Component({
    selector: 'password-show-errors',
    template: `
        <div *ngIf="shouldShowErrors()">
            <mat-error *ngFor="let error of listOfErrors()">{{error}}</mat-error>
        </div>
    `
})
export class ChangePasswordShowErrorsComponent {

    public static readonly errorMessages = {
        'required': () => 'This field is required.',
        'passwordsNotMatching': () => 'Password must match.',
        'newPasswordSameAsOldPassword': () => 'New password must not the same as old password.',
        'passwordMismatch': (param) => param.message,
        'passwordTooShort': (param) => param.message,
        'passwordRequiresNonAlphanumeric': (param) => param.message,
        'passwordRequiresDigit': (param) => param.message,
        'passwordRequiresUpper': (param) => param.message,
        'passwordError': (param) => param.message,
    }

    @Input()
    private control: AbstractControlDirective | AbstractControl;

    shouldShowErrors() : boolean {
        return this.control &&
            this.control.errors &&
            (this.control.dirty || this.control.touched);
    }

    listOfErrors(): string[] {

        var errors = Object.keys(this.control.errors)
                        .map(field => this.getMessage(field, this.control.errors[field]));
        return errors; 
    }

    getMessage(type: string, params: any): any {
        try {
            var errorMessage = ChangePasswordShowErrorsComponent.errorMessages[type](params);
            return errorMessage;
        }catch(ex) {
            console.log(ex);
            console.log('type: ', type);
            console.log('params: ', params);
        }
        
    }
}