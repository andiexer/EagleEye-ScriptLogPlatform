import { NgModule } from '@angular/core';

import { SharedModule } from '../../shared/shared.module';
import { scriptInstanceRouting } from './scriptinstances.routing';
import { ScriptinstancesListComponent } from './scriptinstances-list/scriptinstances-list.component';
import { ScriptinstanceDetailsComponent } from './scriptinstance-details/scriptinstance-details.component';
import { ScriptinstancesComponent } from './scriptinstances.component';

@NgModule({
  imports: [
    SharedModule,
    scriptInstanceRouting
  ],
  declarations: [
    ScriptinstancesComponent,
    ScriptinstanceDetailsComponent,
    ScriptinstancesListComponent,
  ]
})
export class ScriptinstancesModule { }
