import { ScriptinstancesComponent } from './scriptinstances.component';
import { ScriptinstancesListComponent } from './scriptinstances-list/scriptinstances-list.component';
import { ScriptinstanceDetailsComponent } from './scriptinstance-details/scriptinstance-details.component';

import { RouterModule, Routes } from '@angular/router';
const scriptinstanceRoutes: Routes = [
    { path: '', component: ScriptinstancesComponent, children: [
        { path: '', component: ScriptinstancesListComponent },
        { path: ':guid', component: ScriptinstanceDetailsComponent }
    ]}
];
export const scriptInstanceRouting = RouterModule.forChild(scriptinstanceRoutes);
