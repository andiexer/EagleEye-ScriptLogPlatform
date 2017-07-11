import { Observable } from 'rxjs/Rx';
export interface ICanDeactivate {
  canDeactivate: () => boolean | Observable<boolean>;
}
