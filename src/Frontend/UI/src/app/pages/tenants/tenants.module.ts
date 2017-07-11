import { NgModule } from '@angular/core';

import { TenantsComponent } from './tenants.component';
import { SharedModule } from '../../shared/shared.module';
import { TenantsListComponent } from './tenants-list/tenants-list.component';
import { TenantDetailComponent } from './tenant-detail/tenant-detail.component';
import { TenantEditComponent } from './tenant-edit/tenant-edit.component';
import { tenantRouting } from './tenants.routing';
import { TenantResolver } from './tenant.resolver';
import { TenantEditGuard } from './tenant-edit/tenant-edit.guard';

@NgModule({
    imports: [
        SharedModule,
        tenantRouting
    ],
    declarations: [
        TenantsComponent,
        TenantsListComponent,
        TenantDetailComponent,
        TenantEditComponent
    ],
    providers: [
        TenantResolver,
        TenantEditGuard
    ]
})
export class TenantsModule {}
