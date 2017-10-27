import { Router } from '@angular/router';
import { Observable, Subject } from 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { Headers, Http, Response, RequestOptions } from '@angular/http';

import { ConfigService } from './config.service';
import { IScript } from '../interfaces/iscript';
import { IScriptEdit } from '../interfaces/iscript-edit';
import { IScripts } from '../interfaces/iscripts';

@Injectable()
export class ScriptDataService {
  private _baseUrl: string;
  private scriptChangeSource: Subject<IScriptEdit> = new Subject<IScriptEdit>();
  scriptChange = this.scriptChangeSource.asObservable();

  constructor(
    private http: Http,
    private configService: ConfigService,
    private router: Router
  ) {
    this._baseUrl = configService.getApiURI();
  }

  getScripts(scriptname?: string, currentPage?: number, itemsPerPage?: number): Observable<IScripts> {
    let headers = new Headers();
    if (currentPage && itemsPerPage) {
      headers.append('Pagination', currentPage + ',' + itemsPerPage);
    }
    let queryParams = new URLSearchParams();
    queryParams.set('scriptname', scriptname);
    let options = new RequestOptions({ headers: headers, params: queryParams.toString() });
    return this.http.get(this._baseUrl + 'Scripts', options)
      .map((res: Response) => {
        let result: IScripts = {pagination: null, scripts: null};
        result.pagination = JSON.parse(res.headers.get('Pagination'));
        result.scripts = res.json();
        return result;
      })
      .catch((error: Response) => {
        this.router.navigate(['/error'], { queryParams: {
          error: error,
          component: 'ScriptDataService'
        }});
        return Observable.throw(error.json().error || 'Server connection error');
      });
  }

  getScript(id: number): Observable<IScript> {
    return this.http.get(this._baseUrl + 'Scripts/' + id)
      .map((res: Response) => {
        return res.json();
      })
      .catch((error: Response) => {
        this.router.navigate(['/error'], { queryParams: {
          error: error,
          component: 'ScriptDataService'
        }});
        return Observable.throw(error.json().error || 'Server connection error');
      });
  }

  setScript(script: IScriptEdit, id: number): Observable<any> {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    return this.http.put(this._baseUrl + 'Scripts/' + id, script, options)
      .map(() => this.scriptChangeSource.next(script))
      .catch((error: Response) => {
        this.router.navigate(['/error'], { queryParams: {
          error: error,
          component: 'ScriptDataService'
        }});
        return Observable.throw(error.json().error || 'Server connection error');
      });
  }

  addScript(script: IScriptEdit): Observable<any> {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    return this.http.post(this._baseUrl + 'Scripts', script, options)
      .map(() => this.scriptChangeSource.next(script))
      .catch((error: Response) => {
        this.router.navigate(['/error'], { queryParams: {
          error: error,
          component: 'ScriptDataService'
        }});
        return Observable.throw(error.json().error || 'Server connection error');
      });
  }

  removeScript(id: number): Observable<any> {
    return this.http.delete(this._baseUrl + 'Scripts/' + id)
      .catch((error: Response) => {
        this.router.navigate(['/error'], { queryParams: {
          error: error,
          component: 'ScriptDataService'
        }});
        return Observable.throw(error.json().error || 'Server connection error');
      });
  }

}
