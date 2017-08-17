import { ILogs } from '../interfaces/ilogs';
import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import { IScriptInstance } from '../interfaces/iscript-instance';
import { ConfigService } from './config.service';
import { ILog } from '../interfaces/ilog';
import { Router } from '@angular/router';
import { IScriptInstances } from '../interfaces/iscript-instances';

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

  getScriptInstances(
    hostname?: string,
    scriptname?: string,
    transactionId?: string,
    status?: string[],
    from?: Date,
    to?: Date,
    currentPage?: number,
    itemsPerPage?: number
  ): Observable<IScriptInstances> {
    const headers = new Headers();
    if (currentPage && itemsPerPage) {
      headers.append('Pagination', currentPage + ',' + itemsPerPage);
    }
    let queryParams: URLSearchParams = new URLSearchParams();
    queryParams.set('hostname', hostname);
    queryParams.set('scriptname', scriptname);
    queryParams.set('transactionId', transactionId);
    for (let i = 0; i < status.length; i++) {
      queryParams.append('status', status[i]);
    }
    if (from) {
      queryParams.set('from', from.toDateString());
    }
    if (to) {
      queryParams.set('to', to.toDateString());
    }
    const options = new RequestOptions({ headers: headers, params: queryParams.toString() });
    return Observable.timer(0, 5000)
      .flatMap(() => {
        return this.http.get(this._baseUrl + 'ScriptInstances', options)
          .map((res: Response) => {
            let result: IScriptInstances = { pagination: null, scriptInstances: null };
            result.pagination = JSON.parse(res.headers.get('Pagination'));
            result.scriptInstances = res.json();
            return result;
          })
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

  getScriptInstance(id: number): Observable<IScriptInstance> {
    return Observable.timer(0, 10000)
      .flatMap(() => {
        return this.http.get(this._baseUrl + 'ScriptInstances/' + id)
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

  getScriptInstanceLogs(
    id: number,
    logLevel?: string[],
    logText?: string,
    currentPage?: number,
    itemsPerPage?: number
  ): Observable<ILogs> {
    const headers = new Headers();
    if (currentPage && itemsPerPage) {
      headers.append('Pagination', currentPage + ',' + itemsPerPage);
    }
    const queryParams = new URLSearchParams();
    for (let i = 0; i < logLevel.length; i++) {
      queryParams.append('logLevel', logLevel[i]);
    }
    queryParams.set('logText', logText);
    const options = new RequestOptions({ headers: headers, params: queryParams.toString() });
    return Observable.timer(0, 3000)
      .flatMap(() => {
        return this.http.get(this._baseUrl + 'ScriptInstances/' + id + '/Logs', options)
          .map((res: Response) => {
            let result: ILogs = { pagination: null, logs: null };
            result.pagination = JSON.parse(res.headers.get('Pagination'));
            result.logs = res.json();
            return result;
          })
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

  removeScriptInstance(id: number): Observable<any> {
    return this.http.delete(this._baseUrl + 'ScriptInstances/' + id)
      .catch((error: Response) => {
        this.router.navigate(['/error'], {
          queryParams: {
            error: error,
            component: 'ScriptInstanceDataService'
          }
        });
        return Observable.throw(error.json().error || 'Server connection error');
      });
  }

}
