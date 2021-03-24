
import { Directive, ElementRef, OnInit, Output, EventEmitter } from '@angular/core';

// <reference types="@types/googlemaps" />

declare var google: any;

// var southWest = new google.maps.LatLng(54.640301, 112.921112),
//                 northEast = new google.maps.LatLng(9.228820, 159.278717),
//                 australiaBounds = new google.maps.LatLngBounds(southWest, northEast),
//                 componentForm,
//                 options,
//                 autocomplete;
var southWest = new google.maps.LatLng(7.247399919700606, 117.67668359374997),
                northEast = new google.maps.LatLng(19.117781060595696, 126.726677734375),
                philippinesBounds = new google.maps.LatLngBounds(southWest, northEast),
                componentForm,
                options,
                autocomplete;
    debugger;
  componentForm = {
    street_number: "short_name",
    route: "long_name",
    locality: "long_name",
    administrative_area_level_1: "short_name",
    country: "long_name",
    postal_code: "short_name"
};

options = {
    bounds: philippinesBounds,
    types: ["geocode"],
    componentRestrictions: { country: "au" }
};

@Directive({
  selector: '[google-place]'
})
export class GooglePlacesDirective implements OnInit {
  @Output() onSelect: EventEmitter<any> = new EventEmitter();
  private element: HTMLInputElement;
  
  constructor(private elRef: ElementRef) { 
    //elRef will get a reference to the element where
    //the directive is placed
    this.element = elRef.nativeElement;
  }
  
  getFormattedAddress(place) {
    //@params: place - Google Autocomplete place object
    //@returns: location_obj - An address object in human readable format
    let location_obj = {};
    for (let i in place && place.address_components) {
      let item = place.address_components[i];
      
      location_obj['formatted_address'] = place.formatted_address;
      if(item['types'].indexOf("locality") > -1) {
        location_obj['locality'] = item['long_name']
      } else if (item['types'].indexOf("administrative_area_level_1") > -1) {
        location_obj['admin_area_l1'] = item['short_name']
      } else if (item['types'].indexOf("street_number") > -1) {
        location_obj['street_number'] = item['short_name']
      } else if (item['types'].indexOf("route") > -1) {
        location_obj['route'] = item['long_name']
      } else if (item['types'].indexOf("country") > -1) {
        location_obj['country'] = item['long_name']
      } else if (item['types'].indexOf("postal_code") > -1) {
        location_obj['postal_code'] = item['short_name']
      }
     
    }

    return location_obj;
  }

  ngOnInit(): void {
    
    autocomplete = new google.maps.places.Autocomplete(this.element, options);
    //Event listener to monitor place changes in the input
    google.maps.event.addListener(autocomplete, 'place_changed', () => {
      //Emit the new address object for the updated place
      this.onSelect.emit(this.getFormattedAddress(autocomplete.getPlace()));
    });

  }

}
