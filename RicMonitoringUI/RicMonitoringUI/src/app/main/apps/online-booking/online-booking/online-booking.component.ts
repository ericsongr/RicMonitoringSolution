import { Component, OnInit } from '@angular/core';
import { FuseConfigService } from '@fuse/services/config.service';
import { FormBuilder, FormGroup, Validators, FormArray, FormControl } from '@angular/forms';
import {COMMA, ENTER} from '@angular/cdk/keycodes';

import { Country } from '@angular-material-extensions/select-country';
import { MatChipInputEvent } from '@angular/material';
import { Subscription } from 'rxjs';
import { LookupTypeItemsService } from '../../common/services/lookup-type-items.service';
import { toBase64String } from '@angular/compiler/src/output/source_map';
import { OnlineBookingService } from './online-booking.service';

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

  onAgesChanged: Subscription;
  lookupTypeAges: []
  agesDefaultValue: string;

  constructor(
    private _onlineBookingService: OnlineBookingService,
    private _formBuilder: FormBuilder,
    private _fuseConfigService: FuseConfigService) 
    {
      //set window settings
      this.setConfig();

      this.onAgesChanged = 
        this._onlineBookingService.onLookupTypeItemsChanged
            .subscribe(result =>{
                this.lookupTypeAges = result.ages;
                this.agesDefaultValue = result.defaultValue;
            });

           //initialize form
            this.bookingForm = this._formBuilder.group({
              country: ['', Validators.required],
              languagesSpoken: ['', Validators.required],
              email: ['', [Validators.required, Validators.email]],
              contact: ['', Validators.required],
              leaveMessage: [''],
              persons: this._formBuilder.array([])
            });

            //just delay to default value for ages
             setTimeout(() => {
                //add single entry for person by default
                  this.addPerson();
            }, 100);
  }

  get persons() : FormArray {
    debugger;
    return this.bookingForm.get("persons") as FormArray
  }

  newPerson() : FormGroup {
    
    return this._formBuilder.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      ages: [this.agesDefaultValue, Validators.required],
    });
  }

  addPerson() {
    this.persons.push(this.newPerson());
  }

  removePerson(i: number) {
    this.persons.removeAt(i);
  }

  onSubmit() {
    var data = this.bookingForm.value;
    data.languagesSpoken =  this.getLanguagesSpoken(); 
  }

  getLanguagesSpoken(){
    return this.languages.map(function (a) { return a.name; }).join(',');
  }
  
  onCountrySelected($event: Country) {
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
      this.subscribeConfigChanged();
  } 


  subscribeConfigChanged() {

    // Subscribe to config change
    this._fuseConfigService.config
    .subscribe((config) => {
        this.config = config;
    });

  }

}
