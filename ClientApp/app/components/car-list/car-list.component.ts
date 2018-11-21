import { KeyValuePair } from './../../models/car';
import { CarService } from './../../services/car.service';
import { Component, OnInit } from '@angular/core';
import { Car } from '../../models/car';

@Component({
  // selector: 'app-car-list',
  templateUrl: './car-list.component.html',
  // styleUrls: ['./car-list.component.css']
})
export class CarListComponent implements OnInit {
  private readonly PAGE_SIZE = 3;

  queryResult: any = {};
  makes: any;
  query: any = {
    pageSize: this.PAGE_SIZE
  };
  // field to render the columns dynamically. title is what we render in the table and 
  // key is what is send to the server for sorting and isSortable determines if the column is sortable
  columns = [
    { title: 'Id' },
    { title: 'Contact Name', key: 'contactName', isSortable: true },
    { title: 'Make', key: 'make', isSortable: true },
    { title: 'Model', key: 'model', isSortable: true },
    {  } // link to view the car
  ];

constructor(private carService: CarService) { }

  ngOnInit() {
    // initialize the drop down list
    this.carService.getMakes()
        .subscribe(makes => this.makes = makes);

    // method call for filtering
    this.populateCars();
  }

  private populateCars() {  
    this.carService.getCars(this.query)
      .subscribe(result => this.queryResult = result);
  }

  // Implementing the pattern for filtering
  onFilterChange() {
    // resetting the page number
    this.query.page = 1;
    // method call for filtering
    this.populateCars();
  }

  // Reset the filter
  clearFilter() {
    this.query = {
      page: 1,
      // resetting pageSize
      pageSize: this.PAGE_SIZE
    };
    this.populateCars();
  }

  // sort by column name
  sortBy(columnName) {
    if (this.query.sortBy === columnName) {
        // reverse the sort order
        this.query.isSortAscending = !this.query.isSortAscending;
    } else {
        // sort by that column in ascending order
        this.query.sortBy = columnName;
        this.query.isSortAscending = true;
    }
    // populate the Cars
    this.populateCars();
  }

  onPageChange(page) {
    this.query.page = page;
    this.populateCars();
  }
}
