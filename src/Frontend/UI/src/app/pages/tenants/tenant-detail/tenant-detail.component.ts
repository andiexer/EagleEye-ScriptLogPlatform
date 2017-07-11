import { Subscription } from 'rxjs/Rx';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ITenant } from '../../../shared/interfaces/itenant';
import { TenantDataService } from '../../../shared/services/tenant-data.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-tenant-detail',
  templateUrl: './tenant-detail.component.html'
})
export class TenantDetailComponent implements OnInit, OnDestroy {
  private tenantSubscription: Subscription;
  private routeSubscription: Subscription;
  private routeQuerySubscription: Subscription;
  private tenantChangeSubscription: Subscription;
  private tenantRouteSubscription: Subscription;
  private tenantId: number;
  private returnUrl: string;
  public tenant: ITenant;

  constructor(
    private tenantDataService: TenantDataService,
    private router: Router,
    private route: ActivatedRoute,
  ) {
    this.tenantRouteSubscription = this.route.data.subscribe(
      (res: any) => this.tenant = res.tenant
    );
  }

  ngOnInit() {
    this.routeSubscription = this.route.params.subscribe(
      (params: any) => {
        this.tenantId = parseInt(params['id'], 10);
        if (!this.tenant) {
          this.getTenant(this.tenantId);
        }
    });
    this.routeQuerySubscription = this.route.queryParams.subscribe(
      (queryParam: any) => {
        this.returnUrl = queryParam['returnUrl'] || '/tenants';
      });
    this.tenantChangeSubscription = this.tenantDataService.tenantChange.subscribe((res: ITenant) => {
      this.getTenant(this.tenantId);
    });
  }

  ngOnDestroy() {
    if (this.routeSubscription) {
      this.routeSubscription.unsubscribe();
    }
    if (this.tenantSubscription) {
      this.tenantSubscription.unsubscribe();
    }
    if (this.tenantChangeSubscription) {
      this.tenantChangeSubscription.unsubscribe();
    }
    if (this.tenantRouteSubscription) {
      this.tenantRouteSubscription.unsubscribe();
    }
    if (this.routeQuerySubscription) {
      this.routeQuerySubscription.unsubscribe();
    }
  }

  getTenant(id: number) {
    this.tenantSubscription = this.tenantDataService.getTenant(id)
      .subscribe((res: ITenant) => {
        this.tenant = res;
      });
  }

  onClose() {
    this.router.navigateByUrl(this.returnUrl);
  }

  onEdit() {
    this.router.navigate(['/tenants', this.tenantId, 'edit'], { queryParams: { returnUrl: this.router.url}});
  }

}
