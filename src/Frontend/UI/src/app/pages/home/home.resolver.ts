import { Injectable } from '@angular/core';
import { IHost, LogDataService } from '../../shared';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Resolve } from '@angular/router/src/interfaces';
import { LatestLogsComponent } from './latest-logs/latest-logs.component';
import { ILog } from '../../shared/interfaces/ilog';
@Injectable()
export class HomeResolver implements Resolve<ILog[]> {
    constructor(
        private logDataService: LogDataService
    ) {}

    resolve(route: ActivatedRouteSnapshot) {
        return this.logDataService.getLatestLogs(10);
    }

}
