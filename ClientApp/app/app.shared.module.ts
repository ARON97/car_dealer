import { PaginationComponent } from './components/shared/pagination.component';
import { CarListComponent } from './components/car-list/car-list.component';
import * as Raven from 'raven-js';
import { FormsModule } from '@angular/forms';
import { NgModule, ErrorHandler } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ToastyModule } from 'ng2-toasty';

import { CarService } from './services/car.service';
import { CommonModule } from '@angular/common';
import { HttpModule, BrowserXhr } from '@angular/http';

import { CarFormComponent } from './components/car-form/car-form.component'; // added
import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
import { AppErrorHandler } from './app.error-handler';
import { ViewCarComponent } from './components/view-car/view-car.component';
import { PhotoService } from './services/photo.service';
import { BrowserXhrWithProgress, ProgressService } from './services/progress.service';
import { AuthService } from './services/auth.service';

// Add the URL of the project in Sentry
Raven.config('https://e4d049cae99f47abb4d25a07645f53e0@sentry.io/1225978').install();

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        HomeComponent,
        CarFormComponent, // added
        CarListComponent,
        ViewCarComponent,
        PaginationComponent,
    ],
    imports: [
        CommonModule,
        HttpModule,
        ToastyModule.forRoot(),
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'cars', pathMatch: 'full' },
            { path: 'cars/new', component: CarFormComponent },
            { path: 'cars/edit/:id', component: CarFormComponent },
            { path: 'cars/:id', component: ViewCarComponent },
            { path: 'cars', component: CarListComponent },
            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },
            { path: '**', redirectTo: 'cars' }
        ])
    ],
    providers: [
        { provide: ErrorHandler,  useClass: AppErrorHandler},
        { provide: BrowserXhr,  useClass: BrowserXhrWithProgress},
        // service registrations
        CarService,
        PhotoService,
        ProgressService,
        AuthService
    ]
})
export class AppModuleShared {
}
