// external module
import 'hammerjs';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { MaterialModule, MdNativeDateModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';

import { routing } from './app.routing';
import { ErrorComponent } from './pages/error/error.component';
import { AppComponent } from './app.component';
import { CoreModule } from './core.module';
import {
  LogDataService,
  ConfigService,
  ScriptinstanceDataService,
  HostDataService,
  DialogsService,
  ScriptDataService
} from './shared';

let modules = [
  BrowserModule,
  FormsModule,
  ReactiveFormsModule,
  HttpModule,
  CoreModule,
  FlexLayoutModule,
  BrowserAnimationsModule,
  MaterialModule,
  MdNativeDateModule
];

let services = [
  LogDataService,
  ConfigService,
  ScriptinstanceDataService,
  HostDataService,
  ScriptDataService,
  DialogsService
];

export function initializeConfigService(config: ConfigService) { return () => config.load(); }

// main bootstrap
@NgModule({
  declarations: [
    ErrorComponent,
    AppComponent
  ],
  imports: [
    ...modules,
    routing
  ],
  providers: [
    ...services,
    // Load initial config json file
    {
      provide: APP_INITIALIZER,
      useFactory: initializeConfigService,
      deps: [ConfigService],
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
