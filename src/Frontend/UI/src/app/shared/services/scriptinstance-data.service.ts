import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { IScriptInstance } from '../interfaces/iscript-instance';
import { ConfigService } from './config.service';
import { ILog } from '../interfaces/ilog';
import { Router } from '@angular/router';

@Injectable()
export class ScriptinstanceDataService {
  private _baseUrl: string;

  constructor(
    private http: Http,
    private configService: ConfigService,
    private router: Router
  ) {
    this._baseUrl = configService.getApiURI();
  }

  getScriptInstances(): Observable<IScriptInstance[]> {
    return Observable.timer(0, 5000)
      .flatMap(() => {
        return this.http.get(this._baseUrl + 'scriptinstances')
          .map((res: Response) => { return res.json(); })
          .catch((error: Response) => {
            this.router.navigate(['/error'], {
              queryParams: {
                error: error,
                component: 'ScriptinstanceDataService'
              }
            });
            return Observable.throw(error.json().error || 'Server connection error');
          });
      });
  }

  getScriptInstance(guid: string): Observable<IScriptInstance> {
    return Observable.timer(0, 10000)
      .flatMap(() => {
        return this.http.get(this._baseUrl + 'scriptinstances/' + guid)
          .map((res: Response) => { return res.json(); })
          .catch((error: Response) => {
            this.router.navigate(['/error'], {
              queryParams: {
                error: error,
                component: 'ScriptInstanceDataService'
              }
            });
            return Observable.throw(error.json().error || 'Server connection error');
          });
      });
  }

  getScriptInstanceLogs(guid: string): Observable<ILog[]> {
    return Observable.timer(0, 1000)
      .flatMap(() => {
        return this.http.get(this._baseUrl + 'scriptinstances/' + guid + '/logs')
          .map((res: Response) => { return res.json(); })
          .catch((error: Response) => {
            this.router.navigate(['/error'], {
              queryParams: {
                error: error,
                component: 'ScriptInstanceDataService'
              }
            });
            return Observable.throw(error.json().error || 'Server connection error');
          });
      });
  }

}
