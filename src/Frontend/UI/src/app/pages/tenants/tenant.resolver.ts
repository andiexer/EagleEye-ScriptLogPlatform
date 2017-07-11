import { TenantDataService, ITenant } from '../../shared';
import { Resolve } from '@angular/router/src/interfaces';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
@Injectable()
export class TenantResolver implements Resolve<ITenant> {
    constructor(
        private tenantDataService: TenantDataService
    ) {}

    resolve(route: ActivatedRouteSnapshot) {
        return this.tenantDataService.getTenant(parseInt(route.params['id'], 10));
    }

}
