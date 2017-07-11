import { Router } from '@angular/router';
import { Subject } from 'rxjs/Rx';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { ITenant } from '../interfaces/itenant';
import { Headers, Http, RequestOptions, Response } from '@angular/http';
import { ConfigService } from './config.service';

@Injectable()
export class TenantDataService {
  private _baseUrl: string;
  private tenantChangeSource: Subject<ITenant> = new Subject<ITenant>();
  tenantChange = this.tenantChangeSource.asObservable();

  constructor(
    private http: Http,
    private configService: ConfigService,
    private router: Router
  ) {
    this._baseUrl = configService.getApiURI();
  }

  getTenants(): Observable<ITenant[]> {
    return this.http.get(this._baseUrl + 'Tenants')
      .map((res: Response) => {
        return res.json();
      })
      .catch((error: Response) => {
        this.router.navigate(['/error'], {
          queryParams: {
            error: error,
            component: 'TenantDataService'
          }
        });
        return Observable.throw(error.json().error || 'Server connection error');
      });
  }

  getTenant(id: number): Observable<ITenant> {
    return this.http.get(this._baseUrl + 'Tenants/' + id)
      .map((res: Response) => {
        return res.json();
      })
      .catch((error: Response) => {
        this.router.navigate(['/error'], {
          queryParams: {
            error: error,
            component: 'TenantDataService'
          }
        });
        return Observable.throw(error.json().error || 'Server connection error');
      });
  }

  setTenant(tenant: ITenant, tenantId: number): Observable<ITenant> {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    return this.http.put(this._baseUrl + 'Tenants/' + tenantId, tenant, options)
      .map(() => this.tenantChangeSource.next(tenant))
      .catch((error: Response) => {
        this.router.navigate(['/error'], {
          queryParams: {
            error: error,
            component: 'TenantDataService'
          }
        });
        return Observable.throw(error.json().error || 'Server connection error');
      });
  }

  addTenant(tenant: ITenant): Observable<ITenant> {
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });
    return this.http.post(this._baseUrl + 'Tenants', tenant, options)
      .map(() => this.tenantChangeSource.next(tenant))
      .catch((error: Response) => {
        this.router.navigate(['/error'], {
          queryParams: {
            error: error,
            component: 'TenantDataService'
          }
        });
        return Observable.throw(error.json().error || 'Server connection error');
      });
  }

  removeTenant(id: number): Observable<any> {
    return this.http.delete(this._baseUrl + 'Tenants/' + id)
      .catch((error: Response) => {
        this.router.navigate(['/error'], {
          queryParams: {
            error: error,
            component: 'TenantDataService'
          }
        });
        return Observable.throw(error.json().error || 'Server connection error');
      });
  }

}
