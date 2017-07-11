import { Resolve } from '@angular/router/src/interfaces';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { TenantDataService, ITenant } from '../../../shared';
@Injectable()
export class TenantsResolver implements Resolve<ITenant[]> {
    constructor(
        private tenantDataService: TenantDataService
    ) {}

    resolve(route: ActivatedRouteSnapshot) {
        return this.tenantDataService.getTenants();
    }

}
