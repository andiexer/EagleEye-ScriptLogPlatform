import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/Rx';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { HostDataService, IHost, IHosts } from '../../../shared';
import { IHostEdit } from '../../../shared/interfaces/ihost-edit';
import { DialogsService } from '../../../shared/services/dialogs.service';
import { PageEvent } from '@angular/material';

@Component({
  selector: 'app-hosts-list',
  templateUrl: './hosts-list.component.html'
})
export class HostsListComponent implements OnInit, OnDestroy {
  private querySubscription: Subscription;
  private hostChangeSubscription: Subscription;
  private hostSubscription: Subscription;
  public hosts: IHost[];
  public tenantOptions = [];
  public searchHostname: string = '';
  public searchForm: FormGroup;
  public length = 100;
  public pageSize = 10;
  public pageSizeOptions = [5, 10, 25, 100];
  public currentPage: number;
  public loadingHosts: boolean;

  constructor(
    private hostDataService: HostDataService,
    private dialogsService: DialogsService,
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    // get query parameter from active route
    this.querySubscription = this.route.queryParams.subscribe(
      (queryParam: any) => {
        if (queryParam['hostname']) {
          this.searchHostname = queryParam['hostname'];
        }
      });
    this.getHosts();
    // everytime when a host changes refresh the hosts array
    this.hostChangeSubscription = this.hostDataService.hostChange.subscribe((res: IHostEdit) => {
      this.getHosts();
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
  }

  private initForm() {
    let hostname = this.searchHostname;

    this.searchForm = this.formBuilder.group({
      hostname: [hostname],
    });
  }

  getHosts() {
    this.loadingHosts = true;
    this.hostSubscription = this.hostDataService.getHosts(this.searchHostname, this.currentPage + 1, this.pageSize)
      .subscribe(
      (res: IHosts) => {
        this.loadingHosts = false;
        this.hosts = res.hosts;
        this.currentPage = res.pagination.CurrentPage - 1;
        this.pageSize = res.pagination.ItemsPerPage;
        this.length = res.pagination.TotalItems;
      },
      error => {
        console.log(error);
      }
      );
  }

  onSearch() {
    this.searchHostname = this.searchForm.value.hostname;
    let queryParams: any = {};
    if (this.searchHostname) { queryParams.hostname = this.searchHostname; }
    this.router.navigate(['/hosts'], { queryParams: queryParams });
    this.getHosts();
  }

  onSearchClear() {
    this.searchForm.controls['hostname'].setValue('');
    this.onSearch();
  }

  onDetails(id: string) {
    this.router.navigate(['/hosts', id], { queryParams: { returnUrl: this.router.url } });
  }

  onDelete(id: string) {
    this.hostDataService.removeHost(parseInt(id, 10)).subscribe(
      () => {
        this.getHosts();
      });
  }

  onNew() {
    this.router.navigate(['/hosts', 'new'], { queryParams: { returnUrl: this.router.url } });
  }

  onEdit(hostsId: number) {
    this.router.navigate(['/hosts', hostsId, 'edit'], { queryParams: { returnUrl: this.router.url } });
  }

  onPageChange(pageEvent: PageEvent) {
    this.currentPage = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.getHosts();
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
