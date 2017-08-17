import { IHost } from '../../../shared';
import { Resolve } from '@angular/router/src/interfaces';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { HostDataService } from '../../../shared/services/host-data.service';
@Injectable()
export class HostDetailResolver implements Resolve<IHost> {
    constructor(
        private hostDataService: HostDataService
    ) {}

    resolve(route: ActivatedRouteSnapshot) {
        return this.hostDataService.getHost(parseInt(route.params['id'], 10));
    }

}
