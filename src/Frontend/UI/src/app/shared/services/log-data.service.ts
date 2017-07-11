import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs/Rx';
import 'rxjs/add/operator/toPromise';


import { ILog, } from '../';
import { ITotalLogs } from '../interfaces/itotallogs';
import { ConfigService } from './config.service';
import { Router } from '@angular/router';

@Injectable()
export class LogDataService {
  private _baseUrl: string;

  constructor(
    private http: Http,
    private configService: ConfigService,
    private router: Router
  ) {
    this._baseUrl = configService.getApiURI();
  }

  getLatestLogs(totalItems: number): Observable<ILog[]> {
    return Observable.timer(0, 5000)
      .flatMap(() => {
        return this.http.get(this._baseUrl + 'logs/latest/' + totalItems)
          .map((res: Response) => { return res.json(); })
          .catch((error: Response) => {
            this.router.navigate(['/error'], {
              queryParams: {
                error: error,
                component: 'LogDataService'
              }
            });
            return Observable.throw(error.json().error || 'Server connection error');
          });
      });
  }

  getTotalLogs(): Observable<ITotalLogs> {
    return Observable.timer(0, 5000)
      .flatMap(() => {
        return this.http.get(this._baseUrl + 'logs/statistics/total')
          .map((res: Response) => { return res.json(); })
          .catch((error: Response) => {
            this.router.navigate(['/error'], {
              queryParams: {
                error: error,
                component: 'LogDataService'
              }
            });
            return Observable.throw(error.json().error || 'Server connection error');
          });
      });
  }

  getTotalLogsByLoglevel(): Observable<any> {
    return Observable.timer(0, 5000)
      .flatMap(() => {
        return this.http.get(this._baseUrl + 'logs/statistics/total/loglevels')
          .map((res: Response) => { return res.json(); })
          .catch((error: Response) => {
            this.router.navigate(['/error'], {
              queryParams: {
                error: error,
                component: 'LogDataService'
              }
            });
            return Observable.throw(error.json().error || 'Server connection error');
          });
      });
  }
}
