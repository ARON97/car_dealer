import { SaveCar } from "./../models/car";
import { Injectable } from '@angular/core';
import { Http } from '@angular/http'; // Http import
import 'rxjs/add/operator/map'; // map operator

@Injectable()
export class CarService {

  private readonly carsEndpoint = '/api/cars';
  constructor(private http: Http) { }
  
  getFeatures() {
    // return the features api
    return this.http.get('/api/features')
      .map(res => res.json()); // map the response to JSON
  }

  getMakes() {
    return this.http.get('/api/makes')
      .map(res => res.json()); // map the response and map it to JSON 
  }

  // create a car
  create(car) {
    return this.http.post(this.carsEndpoint, car)
      .map(res => res.json()); // map the response to a JSON object
  }

  // Get a car using a specific ID
  getCar(id) {
    return this.http.get(this.carsEndpoint + '/' + id)
      .map(res => res.json());
  }

  // Get a car according to the filter
  getCars(filter) {
    return this.http.get(this.carsEndpoint + '?' + this.toQueryString(filter))
      .map(res => res.json());
  }

  // Gets an object and converts it to a query string
  toQueryString(obj) {
    var parts = new Array;
    // iterate over each property in this object
    for (var property in obj) {
      // get the value of the property
      var value = obj[property];
      // if the is not null and not undefined 
      if (value != null && value != undefined)
        // send it to the server
        parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value));
    }
    // join all the parts in the array
    return parts.join('&');
  }

  // Update a Car
  update(car: SaveCar) {
    return this.http.put('/api/cars/' + car.id, car)
      .map(res => res.json());
  }

  // delete a car
  delete(id) {
    return this.http.delete('api/cars/' + id)
      .map(res => res.json());
  }
}
