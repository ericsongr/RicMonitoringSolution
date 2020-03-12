import { Component, OnInit } from '@angular/core';
import { FuseConfigService } from '@fuse/services/config.service';
import { FormBuilder, FormGroup, Validators, FormArray } from '@angular/forms';
import {COMMA, ENTER} from '@angular/cdk/keycodes';

import { Country } from '@angular-material-extensions/select-country';
import { MatChipInputEvent } from '@angular/material';

export interface Language {
  name: string;
}

@Component({
  selector: 'app-online-booking',
  templateUrl: './online-booking.component.html',
  styleUrls: ['./online-booking.component.scss']
})
export class OnlineBookingComponent implements OnInit {

  languages : Language[] = [];
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];
  languagesSpoken: string;

  countryInput: string;
  bookingForm: FormGroup
  config: any;
  addOnBlur = true;

  constructor(
    private _formBuilder: FormBuilder,
    private _fuseConfigService: FuseConfigService) 
    {
      //set window settings
      this.setConfig();

      this.bookingForm = this._formBuilder.group({
        country: ['', Validators.required],
        languagesSpoken: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        contact: ['', Validators.required],
        leaveMessage: [''],
        persons: this._formBuilder.array([])
      });

      //add single entry for person
      this.addPerson();
  }

  get persons() : FormArray {
    return this.bookingForm.get("persons") as FormArray
  }

  newPerson() : FormGroup {
    return this._formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      ages: ['', Validators.required],
    });
  }

  addPerson() {
    this.persons.push(this.newPerson());
  }

  removePerson(i: number) {
    this.persons.removeAt(i);
  }

  onSubmit() {
    // console.log(this.bookingForm.value);

    var data = this.bookingForm.value;
    data.languagesSpoken =  this.getLanguagesSpoken(); 
    console.log(data);
  }

  getLanguagesSpoken(){
    return this.languages.map(function (a) { return a.name; }).join(',');
  }
  
  onCountrySelected($event: Country) {
    console.log($event);
    this.countryInput = $event.name;
  }


  addLanguage(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;
    
    // Add our fruit
    if ((value || '').trim()) {
      this.languages.push({name: value.trim()});
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }
  }

  removeLanguage(language: Language): void {
    const index = this.languages.indexOf(language);

    if (index >= 0) {
      this.languages.splice(index, 1);
    }
  }
  
  setConfig(){
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

  ngOnInit() {
    // Subscribe to config change
    this._fuseConfigService.config
    .subscribe((config) => {
        this.config = config;
    });
  }

}
