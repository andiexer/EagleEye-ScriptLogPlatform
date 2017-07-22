import { Routes, RouterModule } from '@angular/router';

import { HostEditGuard } from './host-edit/host-edit.guard';
import { HostEditResolver } from './host-edit/host-edit.resolver';
import { HostEditComponent } from './host-edit/host-edit.component';
import { HostDetailResolver } from './host-detail/host-detail.resolver';
import { HostDetailComponent } from './host-detail/host-detail.component';
import { HostsListComponent } from './hosts-list/hosts-list.component';
import { HostsComponent } from './hosts.component';


const hostRoutes: Routes = [
    { path: '', component: HostsComponent, children: [
        { path: '', component: HostsListComponent},
        { path: 'new', component: HostEditComponent, canDeactivate: [HostEditGuard]},
        { path: ':id', component: HostDetailComponent, resolve: {
            host: HostDetailResolver
        }},
        { path: ':id/edit', component: HostEditComponent, resolve: {
            host: HostEditResolver
        }, canDeactivate: [HostEditGuard]}
    ]}
];

export const hostRouting = RouterModule.forChild(hostRoutes);
