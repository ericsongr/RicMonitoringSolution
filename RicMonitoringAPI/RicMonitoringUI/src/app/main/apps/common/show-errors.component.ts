import { Component, Input } from "@angular/core";
import { AbstractControlDirective, AbstractControl } from "@angular/forms";

@Component({
    selector: 'show-errors',
    template: `
        <div *ngIf="shouldShowErrors()">
            <mat-error *ngFor="let error of listOfErrors()">{{error}}</mat-error>
        </div>
    `
})
export class ShowErrorsComponent {

    public static readonly errorMessages = {
        'required': () => 'This field is required.',
        'email': () => 'Please enter a valid email address.',
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
        var errorMessage = ShowErrorsComponent.errorMessages[type](params);
        return errorMessage;
    }
}