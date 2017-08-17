import { Component, OnDestroy, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, Observable } from 'rxjs/Rx';
import { IScript } from '../../../shared/interfaces/iscript';
import { ScriptDataService } from '../../../shared/services/script-data.service';
import { ICanDeactivate } from '../../../shared/interfaces/icandeactivate';

@Component({
  selector: 'app-script-edit',
  templateUrl: './script-edit.component.html'
})
export class ScriptEditComponent implements OnInit, OnDestroy, ICanDeactivate {
  private routeSubscription: Subscription;
  private scriptRouteSubscription: Subscription;
  private routeQuerySubscription: Subscription;
  private scriptId: number;
  private script: IScript;
  private returnUrl: string;
  private formSaved: boolean = false;
  public scriptForm: FormGroup;
  public formFunction: string = 'new';

  constructor(
    private scriptDataService: ScriptDataService,
    private route: ActivatedRoute,
    private router: Router,
    private formBuilder: FormBuilder,
    private location: Location
  ) {
    this.scriptRouteSubscription = this.route.data.subscribe(
      (res: any) => {
        this.script = res.script;
      });
  }

  ngOnInit() {
    this.routeSubscription = this.route.params.subscribe(
      (params: any) => {
        if (params['id']) {
          this.scriptId = parseInt(params['id'], 10);
          this.formFunction = 'edit';
        }
      });
      this.routeQuerySubscription = this.route.queryParams.subscribe(
        (queryParam: any) => {
          this.returnUrl = queryParam['returnUrl'] || '/scripts';
        });
    this.initForm();
  }

  ngOnDestroy() {
    if (this.routeSubscription) { this.routeSubscription.unsubscribe(); }
    if (this.scriptRouteSubscription) { this.scriptRouteSubscription.unsubscribe(); }
    if (this.routeQuerySubscription) { this.routeQuerySubscription.unsubscribe(); }
  }

  private initForm() {
    let scriptName: string = '';
    let description: string = '';
    if (this.script) {
      scriptName = this.script.scriptname;
      description = this.script.description;
    }

    this.scriptForm = this.formBuilder.group({
      scriptName: [scriptName, Validators.required],
      description: [description]
    });
  }

  goBack() {
    this.router.navigateByUrl(this.returnUrl);
  }

  onSave() {
    if (this.formFunction === 'new') {
      this.scriptDataService.addScript(this.scriptForm.value).subscribe(
        (error: any) => {
          console.log(error);
        });
    } else if (this.formFunction === 'edit') {
      this.scriptDataService.setScript(this.scriptForm.value, this.scriptId).subscribe(
        (error: any) => {
          console.log(error);
      });
    }
    this.formSaved = true;
    this.goBack();
  }

  canDeactivate(): Observable<boolean> | boolean {
    if (this.scriptForm.touched && !this.formSaved) {
      return confirm('Are you sure you want to leave?');
    }
    return true;
  }

}
