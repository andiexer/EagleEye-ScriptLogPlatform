import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/Rx';

import { IScript, ScriptDataService } from '../../../shared';
import { IScriptEdit } from '../../../shared/interfaces/iscript-edit';

@Component({
  selector: 'app-script-detail',
  templateUrl: './script-detail.component.html'
})
export class ScriptDetailComponent implements OnInit, OnDestroy {
  private scriptRouteSubscription: Subscription;
  private routeSubscription: Subscription;
  private routeQuerySubscription: Subscription;
  private scriptSubscription: Subscription;
  private scriptChangeSubscription: Subscription;
  private scriptId: number;
  private returnUrl: string;
  public script: IScript;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private scriptDataService: ScriptDataService
  ) {
    this.scriptRouteSubscription = this.route.data.subscribe(
      (res: any) => {
        this.script = res.script;
      });
  }

  ngOnInit() {
    this.routeSubscription = this.route.params.subscribe(
      (params: any) => {
        this.scriptId = params['id'];
        if (!this.script) {
          this.getScript(this.scriptId);
        }
      });
    this.scriptChangeSubscription = this.scriptDataService.scriptChange.subscribe((res: IScriptEdit) => {
      this.getScript(this.scriptId);
    });
    this.routeQuerySubscription = this.route.queryParams.subscribe(
      (queryParam: any) => {
        this.returnUrl = queryParam['returnUrl'] || '/scripts';
      });
  }

  ngOnDestroy() {
    if (this.routeSubscription) { this.routeSubscription.unsubscribe(); }
    if (this.routeQuerySubscription) { this.routeQuerySubscription.unsubscribe(); }
    if (this.scriptChangeSubscription) { this.scriptChangeSubscription.unsubscribe(); }
    if (this.scriptRouteSubscription) { this.scriptRouteSubscription.unsubscribe(); }
    if (this.scriptSubscription) { this.scriptSubscription.unsubscribe(); }
  }

  getScript(id: number) {
    this.scriptSubscription = this.scriptDataService.getScript(id)
      .subscribe((res: IScript) => {
        this.script = res;
      });
  }

  goBack() {
    this.router.navigateByUrl(this.returnUrl);
  }

  onEdit() {
    this.router.navigate(['/scripts', this.scriptId, 'edit'], { queryParams: { returnUrl: this.router.url } });
  }

}
