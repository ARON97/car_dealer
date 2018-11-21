import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { BrowserXhr } from '@angular/http';

@Injectable()
export class ProgressService {
    uploadProgress: Subject<any>;
    
    startTracking() {
        this.uploadProgress = new Subject();
        return this.uploadProgress;
    }

    notify(progress) {
        this.uploadProgress.next(progress);
    }

    endTracking() {
        this.uploadProgress.complete();
    }
}

// Access XMLHttpRequest
@Injectable()
export class BrowserXhrWithProgress extends BrowserXhr {

    constructor(private service: ProgressService) { super(); }

    build(): XMLHttpRequest {
        // get the XMLHttpRequest object
        var xhr: XMLHttpRequest = super.build();
        
        // Upload progress
        xhr.upload.onprogress = (event) => {
            this.service.notify(this.createProgress(event));
        };

        // upload completion
        xhr.upload.onloadend = () => {
            this.service.endTracking();
        }

        return xhr;
    }

    private createProgress(event) {
        return {
            total: event.total, 
            percentage: Math.round(event.loaded / event.total * 100)
        };
    }
}