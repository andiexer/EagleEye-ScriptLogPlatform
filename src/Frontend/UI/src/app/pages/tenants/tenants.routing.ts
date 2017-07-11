import { Routes, RouterModule } from '@angular/router';

import { TenantsComponent } from './tenants.component';
import { TenantDetailComponent } from './tenant-detail/tenant-detail.component';
import { TenantEditComponent } from './tenant-edit/tenant-edit.component';
import { TenantResolver } from './tenant.resolver';
import { TenantEditGuard } from './tenant-edit/tenant-edit.guard';

const tenantRoutes: Routes = [
    { path: '', component: TenantsComponent, children: [
        { path: '' },
        { path: 'new', component: TenantEditComponent, canDeactivate: [TenantEditGuard] },
        { path: ':id', component: TenantDetailComponent, resolve: {tenant: TenantResolver} },
        { path: ':id/edit', component: TenantEditComponent, resolve: {tenant: TenantResolver}, canDeactivate: [TenantEditGuard] }
    ]}
];

export const tenantRouting = RouterModule.forChild(tenantRoutes);
