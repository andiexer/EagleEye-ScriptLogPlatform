import { ConfigService } from '../../shared';
import { Subscription } from 'rxjs/Rx';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styles: []
})
export class ErrorComponent implements OnInit, OnDestroy {
  private routeSubscription: Subscription;
  private error: string = 'There is no error text.';
  private component: string = 'ErrorComponent';
  public showMore: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private configService: ConfigService
  ) { }

  ngOnInit() {
    this.routeSubscription = this.route.queryParams.subscribe(
      (params: any) => {
        if (params['error']) {
          this.error = params['error'];
        }
        if (params['component']) {
          this.component = params['component'];
        }
      });
  }

  ngOnDestroy() {
    if (this.routeSubscription) {
      this.routeSubscription.unsubscribe();
    }
  }

}
