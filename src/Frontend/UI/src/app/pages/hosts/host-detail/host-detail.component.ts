import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/Rx';
import { ActivatedRoute, Router } from '@angular/router';
import { HostDataService } from '../../../shared/services/host-data.service';
import { IHost } from '../../../shared/interfaces/ihost';
import { IHostEdit } from '../../../shared/interfaces/ihost-edit';

@Component({
  selector: 'app-host-detail',
  templateUrl: './host-detail.component.html'
})
export class HostDetailComponent implements OnInit, OnDestroy {
  private hostRouteSubscription: Subscription;
  private routeSubscription: Subscription;
  private routeQuerySubscription: Subscription;
  private hostSubscription: Subscription;
  private hostChangeSubscription: Subscription;
  private hostId: number;
  private returnUrl: string;
  public host: IHost;
  public tenantOptions = [];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private hostDataService: HostDataService
  ) {
    this.hostRouteSubscription = this.route.data.subscribe(
      (res: any) => {
        this.host = res.host;
      });
  }

  ngOnInit() {
    this.routeSubscription = this.route.params.subscribe(
      (params: any) => {
        this.hostId = params['id'];
        if (!this.host) {
          this.getHost(this.hostId);
        }
      });
    this.routeQuerySubscription = this.route.queryParams.subscribe(
      (queryParam: any) => {
        this.returnUrl = queryParam['returnUrl'] || '/hosts';
      });
    this.hostChangeSubscription = this.hostDataService.hostChange.subscribe((res: IHostEdit) => {
      this.getHost(this.hostId);
    });
  }

  ngOnDestroy() {
    if (this.routeSubscription) { this.routeSubscription.unsubscribe(); }
    if (this.routeQuerySubscription) { this.routeQuerySubscription.unsubscribe(); }
    if (this.hostRouteSubscription) { this.hostRouteSubscription.unsubscribe(); }
    if (this.hostChangeSubscription) { this.hostChangeSubscription.unsubscribe(); }
    if (this.hostSubscription) { this.hostSubscription.unsubscribe(); }
  }

  getHost(id: number) {
    this.hostSubscription = this.hostDataService.getHost(id)
      .subscribe((res: IHost) => {
        this.host = res;
      });
  }

  goBack() {
    this.router.navigateByUrl(this.returnUrl);
  }

  onEdit() {
    this.router.navigate(['/hosts', this.hostId, 'edit'], { queryParams: { returnUrl: this.router.url } });
  }

}
