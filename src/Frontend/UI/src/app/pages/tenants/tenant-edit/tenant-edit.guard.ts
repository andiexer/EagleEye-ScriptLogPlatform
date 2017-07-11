import { CanDeactivate } from '@angular/router';
import { Observable } from 'rxjs/Rx';
import { ICanDeactivate } from '../../../shared';

export class TenantEditGuard implements CanDeactivate<ICanDeactivate> {
    canDeactivate(component: ICanDeactivate): boolean | Observable<boolean> {
        return component.canDeactivate ? component.canDeactivate() as Observable<boolean> | boolean : true;
    }
}
