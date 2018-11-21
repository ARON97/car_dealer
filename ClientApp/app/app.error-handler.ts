import * as Raven from 'raven-js';
import { ToastyService } from 'ng2-toasty';
import { ErrorHandler, Inject, NgZone, isDevMode } from "@angular/core";

export class AppErrorHandler implements ErrorHandler {
    constructor(
        @Inject(NgZone) private ngZone: NgZone,
        @Inject(ToastyService) private toastyService: ToastyService){
    }
    // implement interface ErrorHandler
    handleError(error: any): void {
        // handling the error with Toasty
        this.ngZone.run(() => {
            this.toastyService.error({
                title: 'Error',
                msg: 'An unexpected error happened.',
                theme: 'bootstrap',
                showClose: true,
                timeout: 5000
            });  
        });

        // when we not in the development but in production
        if (!isDevMode())
            Raven.captureException(error.originalError || error);
        else
            throw error; // throws the exception to the console
            
    }

}