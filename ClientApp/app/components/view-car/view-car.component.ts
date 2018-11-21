import { ProgressService } from './../../services/progress.service';
import { PhotoService } from './../../services/photo.service';
import { CarService } from './../../services/car.service';
import { ToastyService } from 'ng2-toasty';
import { Component, OnInit, ElementRef, ViewChild, NgZone } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-view-car',
  templateUrl: './view-car.component.html',
  styleUrls: ['./view-car.component.css']
})
export class ViewCarComponent implements OnInit {
  @ViewChild('fileInput') fileInput: ElementRef;
  car: any;
  carId: number;
  photos: any[];
  progress: any;

  constructor(
    private zone: NgZone,
    private route: ActivatedRoute,
    private router: Router,
    private toasty: ToastyService,
    private progressService: ProgressService,
    private photoService: PhotoService,
    private carService: CarService) { 

      // subscribe to the route parameters
      route.params.subscribe(p => {
        this.carId = +p['id'];
        // carId is not a number or less or equal to 0
        if (isNaN(this.carId) || this.carId <= 0) {
          router.navigate(['/cars']);
          return;
        }
      });
    }

  ngOnInit() {
  
    this.photoService.getPhotos(this.carId)
        .subscribe(photos => this.photos = photos);
    
    this.carService.getCar(this.carId)
      .subscribe(
        c => this.car = c,
        err => {
          if (err.status == 404) {
            this.router.navigate(['/cars']);
            return
          }
        });
  }

  delete() {
    // deletion confirmation
    if (confirm("Are you sure?")) {
      this.carService.delete(this.car.id)
        .subscribe(x => {
          // redirect the user
          this.router.navigate(['/home']);
        });
    }
  }

  uploadPhoto() {
    // progressService
    this.progressService.startTracking()
      .subscribe(progress => {
          console.log(progress);
          this.zone.run(() => {
            this.progress = progress;
          })
        },
      null,
      () => { this.progress = null; });
    // Get a reference to the file input
    var nativeElement:HTMLInputElement = this.fileInput.nativeElement;
    var file = nativeElement.files[0];
    nativeElement.value = '';
    // upload the file to the server
    this.photoService.upload(this.carId, file)
      .subscribe(photo => {
          this.photos.push(photo);
      },
      err => {
        this.toasty.error({
          title: 'Error',
          msg: err.text(),
          theme: 'bootstrap',
          showClose: true,
          timeout: 5000
      }); 
      }
    );
  }

}
