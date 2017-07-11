import { Location } from '@angular/common';
import { ICanDeactivate } from '../../../shared/interfaces/icandeactivate';
import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { Observable, Subscription } from 'rxjs/Rx';
import { TenantDataService } from '../../../shared/services/tenant-data.service';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ITenant } from '../../../shared';

@Component({
  selector: 'app-tenant-edit',
  templateUrl: './tenant-edit.component.html'
})
export class TenantEditComponent implements OnInit, OnDestroy, ICanDeactivate {
  private routeSubscription: Subscription;
  private routeQuerySubscription: Subscription;
  private tenantRouteSubscription: Subscription;
  private tenantId: number;
  private tenant: ITenant;
  private formSaved: Boolean = false;
  private returnUrl: string;
  public formFunction: String = 'new';
  public tenantForm: FormGroup;

  constructor(
    private tenantDataService: TenantDataService,
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder
  ) {
    this.tenantRouteSubscription = this.route.data.subscribe(
      (res: any) => this.tenant = res.tenant
    );
  }

  ngOnInit() {
    this.routeSubscription = this.route.params.subscribe(
      (params: any) => {
        if (params['id']) {
          this.tenantId = parseInt(params['id'], 10);
          this.formFunction = 'edit';
        }
    });
    this.routeQuerySubscription = this.route.queryParams.subscribe(
      (queryParam: any) => {
        this.returnUrl = queryParam['returnUrl'] || '/tenants';
      });
    this.initForm();
  }

  ngOnDestroy() {
    if (this.tenantRouteSubscription) {
      this.tenantRouteSubscription.unsubscribe();
    }
    if (this.routeSubscription) {
      this.routeSubscription.unsubscribe();
    }
    if (this.routeQuerySubscription) {
      this.routeQuerySubscription.unsubscribe();
    }
  }

  private initForm() {
    let tenantName = '';
    if (this.tenant) {
      tenantName = this.tenant.tenantName;
    }

    this.tenantForm = this.formBuilder.group({
      tenantName: [tenantName, Validators.required, this.tenantExistsValidator.bind(this), { debounce: 2000 }]
    });
  }

  goBack() {
    this.router.navigateByUrl(this.returnUrl);
  }

  onSave() {
    if (this.formFunction === 'new') {
      this.tenantDataService.addTenant(<ITenant>this.tenantForm.value).subscribe(
        (error: any) => {
          console.log(error);
        });
    } else if (this.formFunction === 'edit') {
      this.tenantDataService.setTenant(<ITenant>this.tenantForm.value, this.tenantId).subscribe(
        (error: any) => {
          console.log(error);
        });
    }
    this.formSaved = true;
    this.goBack();
  }

  tenantExistsValidator(control: FormControl): Promise<any> | Observable<any> {
    return new Promise(
      (resolve, reject) => {
        this.tenantDataService.getTenants().subscribe(
          (res: ITenant[]) => {
            if (res.some(object => object.tenantName === control.value && object.id === this.tenantId)) {
              resolve(null);
            } else if (res.some(object => object.tenantName === control.value)) {
              resolve({exists: true});
            } else {
              resolve(null);
            }
          },
            err => {
              console.log(err);
              resolve({invalid: true});
          });
      });
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.tenantForm.touched && !this.formSaved) {
      return confirm('Are you sure you want to leave?');
    }
    return true;
  }

}
