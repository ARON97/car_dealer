import { CarService } from './../../services/car.service';
import { Component, OnInit } from '@angular/core';
import { ToastyService } from 'ng2-toasty';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/Observable/forkJoin';
import { SaveCar, Car } from './../../models/car';
import * as _ from 'underscore';

@Component({
  selector: 'app-car-form',
  templateUrl: './car-form.component.html',
  styleUrls: ['./car-form.component.css']
})
export class CarFormComponent implements OnInit {
  makes: any;
  models: any;
  features: any; 
  car: SaveCar = {
    id: 0,
    makeId: 0,
    modelId: 0,
    isRegistered: false,
    features: [],
    contact: {
      name: '',
      email: '',
      phone: '',
    }
  };

  constructor(
    private route: ActivatedRoute, // reads the route parameters
    private router: Router, // navigate the user to a new page if an invalid id is passed
    private carService: CarService,
    private toastyService: ToastyService) {

        // subscribe to route params observable
        route.params.subscribe(p => {
          this.car.id = +p['id'] || 0;
        });
     }

  ngOnInit() {
    
    var sources = [
      this.carService.getMakes(),
      this.carService.getFeatures(),
    ];

    // the id is not 0
    if (this.car.id)
      sources.push(this.carService.getCar(this.car.id));

    Observable.forkJoin(sources).subscribe(data => {
      this.makes = data[0];
      this.features = data[1];
      // when we get the result we check for the id
      if (this.car.id)
      {
        // calling setCar
        this.setCar(data[2]);
        // the models drop-down list is populated based on the Make
        this.populateModels();
      }
    }, err => {
      if (err.status == 404)
          this.router.navigate(['/home']);
    });
      
  }

  private setCar(c: Car) {
    this.car.id = c.id;
    this.car.makeId = c.make.id;
    this.car.modelId = c.model.id;
    this.car.isRegistered = c.isRegistered;
    this.car.contact = c.contact;
    this.car.features = _.pluck(c.features, 'id');
  }

  // the onMakeChange() Method for cascading the drop-down list
  onMakeChange() {
    // call the method
    this.populateModels();
    // clear the model id
    delete this.car.modelId;
  }

  private populateModels() {
     // find the make with a given ID and get the module
     var selectedMake = this.makes.find(m => m.id == this.car.makeId);
     // using selectedMake to populate our second drop-down list
     // if we have a selected make then we choose the models else use an empty array
     this.models = selectedMake ? selectedMake.models : [];
  }

  // Method to determine if the checkbox is checked
  onFeatureToggle(featureId, $event) {
    // if the checkbox is checked, push the feature id into the feature array
    if ($event.target.checked)
      this.car.features.push(featureId);
    else {
      // inorder to remove an object from an array we need to find the index
      var index = this.car.features.indexOf(featureId);
      // remove the index
      this.car.features.splice(index, 1)
    }
  }

  submit() {
    var result$ = (this.car.id) ? this.carService.update(this.car) : this.carService.create(this.car); 
    result$.subscribe(car => {
      this.toastyService.success({
        title: 'Success', 
        msg: 'Data was sucessfully saved.',
        theme: 'bootstrap',
        showClose: true,
        timeout: 5000
      });
      this.router.navigate(['/cars/', car.id])
    });
  }
}
