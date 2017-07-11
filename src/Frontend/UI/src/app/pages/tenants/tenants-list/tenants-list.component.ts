import { Router } from '@angular/router';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/Rx';
import { ITenant, TenantDataService } from '../../../shared';
import { DialogsService } from '../../../shared/services/dialogs.service';

@Component({
  selector: 'app-tenants-list',
  templateUrl: './tenants-list.component.html'
})
export class TenantsListComponent implements OnInit, OnDestroy {
  private tenantSubscription: Subscription;
  private tenantChangeSubscription: Subscription;
  public tenants: ITenant[];
  public currentPage;

  constructor(
    private tenantDataService: TenantDataService,
    private dialogsService: DialogsService,
    private router: Router
  ) { }

  ngOnInit() {
    this.getTenants();
    this.tenantChangeSubscription = this.tenantDataService.tenantChange.subscribe((res: ITenant) => {
      this.getTenants();
    });
  }

  ngOnDestroy() {
    if (this.tenantSubscription) {
      this.tenantSubscription.unsubscribe();
    }
    if (this.tenantChangeSubscription) {
      this.tenantChangeSubscription.unsubscribe();
    }
  }

  getTenants() {
    this.tenantSubscription = this.tenantDataService.getTenants()
      .subscribe((res: ITenant[]) => {
        this.tenants = res;
      });
  }

  onDetails(id: string) {
    this.router.navigate(['/tenants', id], { queryParams: { returnUrl: this.router.url}});
  }

  onDelete(id: string) {
    this.tenantDataService.removeTenant(parseInt(id, 10)).subscribe(
      () => {
        this.getTenants();
      });
  }

  onNew() {
    this.router.navigate(['/tenants/new'], { queryParams: { returnUrl: this.router.url}});
  }

  onEdit(tenantId: number) {
    this.router.navigate(['/tenants', tenantId, 'edit'], { queryParams: { returnUrl: this.router.url}});
  }

  openDialog(id: string) {
    this.dialogsService
      .confirm('Host delete', 'Are you sure you want to delete this host?')
      .subscribe(res => {
        if (res === true) {
          this.onDelete(id);
        }
      });
  }

}
