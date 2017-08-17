import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot } from '@angular/router';
import { Resolve } from '@angular/router/src/interfaces';
import { IScript } from '../../../shared/interfaces/iscript';
import { ScriptDataService } from '../../../shared/services/script-data.service';

@Injectable()
export class ScriptDetailResolver implements Resolve<IScript> {
    constructor(
        private scriptDataService: ScriptDataService
    ) {}

    resolve(route: ActivatedRouteSnapshot) {
        return this.scriptDataService.getScript(parseInt (route.params['id'], 10));
    }
}
