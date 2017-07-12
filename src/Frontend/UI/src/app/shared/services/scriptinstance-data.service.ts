import { Injectable } from '@angular/core';
import { Http, Response, RequestOptions, Headers } from '@angular/http';
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

  getScriptInstances(
    hostname?: string,
    scriptname?: string,
    status?: string[],
    from?: Date,
    to?: Date,
    currentPage?: number,
    itemsPerPage?: number
  ): Observable<IScriptInstance[]> {
    const headers = new Headers();
    if (currentPage && itemsPerPage) {
      headers.append('Pagination', currentPage + ',' + itemsPerPage);
    }
    const queryParams = new URLSearchParams();
    queryParams.append('hostname', hostname);
    queryParams.append('scriptname', scriptname);
    for (const item in status) {
      queryParams.append('status', item);
    }
    queryParams.append('from', from.toDateString());
    queryParams.append('to', from.toDateString());
    const options = new RequestOptions({ headers: headers, params: queryParams });
    return Observable.timer(0, 5000)
      .flatMap(() => {
        return this.http.get(this._baseUrl + 'ScriptInstances', options)
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

  getScriptInstanceLogs(id: number, logLevel: string[], logText: string, currentPage?: number, itemsPerPage?: number): Observable<ILog[]> {
    const headers = new Headers();
    if (currentPage && itemsPerPage) {
      headers.append('Pagination', currentPage + ',' + itemsPerPage);
    }
    const queryParams = new URLSearchParams();
    for (const item in logLevel) {
      queryParams.append('logLevel', item);
    }
    queryParams.append('logText', logText);
    const options = new RequestOptions({ headers: headers, params: queryParams });
    return Observable.timer(0, 1000)
      .flatMap(() => {
        return this.http.get(this._baseUrl + 'ScriptInstances/' + id + '/Logs', options)
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
