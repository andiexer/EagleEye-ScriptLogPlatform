import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/Rx';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HostDataService, IHost } from '../../../shared';
import { IHostEdit } from '../../../shared/interfaces/ihost-edit';
import { DialogsService } from '../../../shared/services/dialogs.service';

@Component({
  selector: 'app-hosts-list',
  templateUrl: './hosts-list.component.html'
})
export class HostsListComponent implements OnInit, OnDestroy {
  private querySubscription: Subscription;
  private hostChangeSubscription: Subscription;
  private hostSubscription: Subscription;
  private tenantSubscription: Subscription;
  public hosts: IHost[];
  public tenantOptions = [];
  public searchHostname: String = '';
  public searchTenant: String = '';
  public searchForm: FormGroup;
  public currentPage;

  constructor(
    private hostDataService: HostDataService,
    private dialogsService: DialogsService,
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) {
    // get tenant data from router resolver
    this.tenantSubscription = this.route.data.subscribe(
      (res: any) => {
        this.tenantOptions = res.tenants.map(tenant => {
          // {value: 'Created', label: 'Created'},
          return { value: tenant.tenantName, label: tenant.tenantName };
        });
      });
  }

  ngOnInit() {
    this.getHosts();
    // everytime when a host changes refresh the hosts array
    this.hostChangeSubscription = this.hostDataService.hostChange.subscribe((res: IHostEdit) => {
      this.getHosts();
    });
    // get query parameter from active route
    this.querySubscription = this.route.queryParams.subscribe(
      (queryParam: any) => {
        if (queryParam['hostname']) {
          this.searchHostname = queryParam['hostname'];
        }
        if (queryParam['tenant']) {
          this.searchTenant = queryParam['tenant'];
        }
      });
    this.initForm();
  }

  ngOnDestroy() {
    if (this.hostChangeSubscription) {
      this.hostChangeSubscription.unsubscribe();
    }
    if (this.querySubscription) {
      this.querySubscription.unsubscribe();
    }
    if (this.hostSubscription) {
      this.hostSubscription.unsubscribe();
    }
    if (this.tenantSubscription) {
      this.tenantSubscription.unsubscribe();
    }
  }

  private initForm() {
    let hostname = this.searchHostname;
    let tenant = this.searchTenant;

    this.searchForm = this.formBuilder.group({
      hostname: [hostname],
      tenant: [tenant]
    });
  }

  getHosts() {
    this.hostSubscription = this.hostDataService.getHosts()
      .subscribe(
      (res: IHost[]) => {
        this.hosts = res;
      },
      error => {
        console.log(error);
      }
      );
  }

  onSearch() {
    this.searchHostname = this.searchForm.value.hostname;
    this.searchTenant = this.searchForm.value.tenant;
    let queryParams: any = {};
    if (this.searchHostname) { queryParams.hostname = this.searchHostname; }
    if (this.searchTenant) { queryParams.tenant = this.searchTenant; }
    this.router.navigate(['/hosts'], { queryParams: queryParams });
  }

  onSearchClear() {
    this.searchForm.controls['hostname'].setValue('');
    this.searchForm.controls['tenant'].setValue('');
    this.onSearch();
  }

  onDetails(id: string) {
    this.router.navigate(['/hosts', id], { queryParams: { returnUrl: this.router.url}});
  }

  onDelete(id: string) {
    this.hostDataService.removeHost(parseInt(id, 10)).subscribe(
      () => {
        this.getHosts();
      });
  }

  onNew() {
    this.router.navigate(['/hosts', 'new'], { queryParams: { returnUrl: this.router.url}});
  }

  onEdit(hostsId: number) {
    this.router.navigate(['/hosts', hostsId, 'edit'], { queryParams: { returnUrl: this.router.url}});
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
