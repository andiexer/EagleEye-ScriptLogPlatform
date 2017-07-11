import { Location } from '@angular/common';
import { IHost } from '../../../shared/interfaces/ihost';
import { HostDataService, ICanDeactivate } from '../../../shared';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription, Observable } from 'rxjs/Rx';
import { ActivatedRoute, Router } from '@angular/router';
import { IHostEdit } from '../../../shared/interfaces/ihost-edit';

@Component({
  selector: 'app-host-edit',
  templateUrl: './host-edit.component.html'
})
export class HostEditComponent implements OnInit, OnDestroy, ICanDeactivate {
  private routeSubscription: Subscription;
  private routeQuerySubscription: Subscription;
  private hostRouteSubscription: Subscription;
  private hostId: number;
  private host: IHost;
  private formSaved: boolean = false;
  private returnUrl: string;
  public hostForm: FormGroup;
  public formFunction: string = 'new';
  public tenantOptions = [];

  constructor(
    private hostDataService: HostDataService,
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder
  ) {
    this.hostRouteSubscription = this.route.data.subscribe(
      (res: any) => {
        this.host = res.host;
        this.tenantOptions = res.tenants.map(tenant => {
          return { value: tenant.id.toString(), label: tenant.tenantName };
        });
      });
  }

  ngOnInit() {
    this.routeSubscription = this.route.params.subscribe(
      (params: any) => {
        if (params['id']) {
          this.hostId = parseInt(params['id'], 10);
          this.formFunction = 'edit';
        }
      });
    this.routeQuerySubscription = this.route.queryParams.subscribe(
      (queryParam: any) => {
        this.returnUrl = queryParam['returnUrl'] || '/hosts';
      });
    this.initForm();
  }

  ngOnDestroy() {
    if (this.hostRouteSubscription) { this.hostRouteSubscription.unsubscribe(); }
    if (this.routeSubscription) { this.routeSubscription.unsubscribe(); }
    if (this.routeQuerySubscription) { this.routeQuerySubscription.unsubscribe(); }
  }

  private initForm() {
    let hostName: string = '';
    let hostDomain: string = '';
    let cloudZone: string = '';
    let powershellVersion: string = '';
    let tenantId: string;
    if (this.host) {
      hostName = this.host.hostName;
      hostDomain = this.host.domain;
      cloudZone = this.host.cloudZone;
      powershellVersion = this.host.powershellVersion;
      tenantId = this.host.tenant.id.toString();
    }

    this.hostForm = this.formBuilder.group({
      hostName: [hostName, Validators.required],
      domain: [hostDomain, Validators.required],
      cloudZone: [cloudZone],
      powershellVersion: [powershellVersion, Validators.required],
      tenantId: [tenantId, Validators.required]
    });
  }

  goBack() {
    this.router.navigateByUrl(this.returnUrl);
  }

  onSave() {
    console.log(this.hostForm);
    if (this.formFunction === 'new') {
      this.hostDataService.addHost(this.hostForm.value).subscribe(
        (error: any) => {
          console.log(error);
        });
    } else if (this.formFunction === 'edit') {
      this.hostDataService.setHost(this.hostForm.value, this.hostId).subscribe(
        (error: any) => {
          console.log(error);
        });
    }
    this.formSaved = true;
    this.goBack();
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.hostForm.touched && !this.formSaved) {
      return confirm('Are you sure you want to leave?');
    }
    return true;
  }

}
