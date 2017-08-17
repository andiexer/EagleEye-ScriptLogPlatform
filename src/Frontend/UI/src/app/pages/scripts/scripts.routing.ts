import { RouterModule, Routes } from '@angular/router';

import { ScriptsComponent } from './scripts.component';
import { ScriptsListComponent } from './scripts-list/scripts-list.component';
import { ScriptDetailResolver } from './script-detail/script-detail.resolver';
import { ScriptDetailComponent } from './script-detail/script-detail.component';
import { ScriptEditComponent } from './script-edit/script-edit.component';
import { ScriptEditGuard } from './script-edit/script-edit.guard';

const scriptRoutes: Routes = [
    { path: '', component: ScriptsComponent, children: [
        { path: '', component: ScriptsListComponent },
        { path: 'new', component: ScriptEditComponent, canDeactivate: [ScriptEditGuard] },
        { path: ':id', component: ScriptDetailComponent, resolve: {
            script: ScriptDetailResolver
        } },
        { path: ':id/edit', component: ScriptEditComponent, resolve: {
            script: ScriptDetailResolver
        }, canDeactivate: [ScriptEditGuard] }
    ]}
];
export const scriptRouting = RouterModule.forChild(scriptRoutes);
