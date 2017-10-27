import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';
import { scriptInstanceRouting } from './scriptinstances.routing';
import { ScriptinstancesListComponent } from './scriptinstances-list/scriptinstances-list.component';
import { ScriptinstanceDetailsComponent } from './scriptinstance-details/scriptinstance-details.component';
import { ScriptinstancesComponent } from './scriptinstances.component';
import {
  MatButtonModule,
  MatCardModule,
  MatIconModule,
  MatSelectModule,
  MatPaginatorModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatInputModule
} from '@angular/material';
import { MomentModule } from 'angular2-moment';

@NgModule({
  imports: [
    SharedModule,
    scriptInstanceRouting,
    MatIconModule,
    MatCardModule,
    MatButtonModule,
    MatSelectModule,
    MatPaginatorModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
    MomentModule
  ],
  declarations: [
    ScriptinstancesComponent,
    ScriptinstanceDetailsComponent,
    ScriptinstancesListComponent,
  ]
})
export class ScriptinstancesModule { }
