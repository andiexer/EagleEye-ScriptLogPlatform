import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

@Injectable()
export class ConfigService {
  private _apiURI: string;
  private version: string = 'v0.0.1';

  constructor(
    private http: Http
  ) { }

  getApiURI() {
    return this._apiURI;
  }

  getApiHost() {
    return this._apiURI.replace('api/', '');
  }

  getVersion() {
    return this.version;
  }

  load() {
    return new Promise((resolve, reject) => {
      this.http.get('assets/appconfig.json')
        .map(res => res.json())
        .subscribe((data) => {
          this._apiURI = data.apiURI;
          if (this._apiURI.slice(-1) !== '/') {
            this._apiURI += '/';
          }
          resolve(true);
        });
    });
  }
}
