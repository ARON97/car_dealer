import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

@Injectable()
export class PhotoService {

    constructor(private http: Http) { }

    upload(carId, photo) {
        var formData = new FormData();
        formData.append('file', photo);
        // send an http request to the server to post a photo
        return this.http.post(`/api/cars/${carId}/photos`, formData)
            .map(res => res.json());
    }

    getPhotos(carId) {
        // send an http get request to the server to get the photos
        return this.http.get(`/api/cars/${carId}/photos`)
            .map(res => res.json());
    }
}