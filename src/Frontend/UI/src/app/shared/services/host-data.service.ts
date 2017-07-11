import { Router } from '@angular/router';
import { Subject } from 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { IHost } from '../interfaces/ihost';
import { Headers, Http, Response, RequestOptions } from '@angular/http';
import { ConfigService } from './config.service';
import { Observable } from 'rxjs/Rx';
import { IHostEdit } from '../interfaces/ihost-edit';

@Injectable()
export class HostDataService {
  private _baseUrl: string;
  private hostChangeSource: Subject<IHostEdit> = new Subject<IHostEdit>();
  hostChange = this.hostChangeSource.asObservable();

  constructor(
    private http: Http,
    private configService: ConfigService,
    private router: Router
  ) {
    this._baseUrl = configService.getApiURI();
  }

  getHosts(): Observable<IHost[]> {
    return this.http.get(this._baseUrl + 'Hosts')
      .map((res: Response) => {
        return res.json();
      })
      .catch((error: Response) => {
        this.router.navigate(['/error'], { queryParams: {
          error: error,
          component: 'HostDataService'
        }});
        return Observable.throw(error.json().error || 'Server connection error');
      });
  }

  getHost(id: number): Observable<IHost> {
    return this.http.get(this._baseUrl + 'Hosts/' + id)
      .map((res: Response) => {
        return res.json();
      })
      .catch((error: Response) => {
        this.router.navigate(['/error'], { queryParams: {
          error: error,
          component: 'HostDataService'
        }});
        return Observable.throw(error.json().error || 'Server connection error');
      });
  }

  setHost(host: IHostEdit, hostId: number): Observable<any> {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    return this.http.put(this._baseUrl + 'Hosts/' + hostId, host, options)
      .map(() => this.hostChangeSource.next(host))
      .catch((error: Response) => {
        this.router.navigate(['/error'], { queryParams: {
          error: error,
          component: 'HostDataService'
        }});
        return Observable.throw(error.json().error || 'Server connection error');
      });
  }

  addHost(host: IHostEdit): Observable<any> {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    return this.http.post(this._baseUrl + 'Hosts', host, options)
      .map(() => this.hostChangeSource.next(host))
      .catch((error: Response) => {
        this.router.navigate(['/error'], { queryParams: {
          error: error,
          component: 'HostDataService'
        }});
        return Observable.throw(error.json().error || 'Server connection error');
      });
  }

  removeHost(id: number): Observable<any> {
    return this.http.delete(this._baseUrl + 'Hosts/' + id)
      .catch((error: Response) => {
        this.router.navigate(['/error'], { queryParams: {
          error: error,
          component: 'HostDataService'
        }});
        return Observable.throw(error.json().error || 'Server connection error');
      });
  }

}
