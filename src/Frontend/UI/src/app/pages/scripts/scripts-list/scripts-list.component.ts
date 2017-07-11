import { isNullOrUndefined } from 'util';
import { Router, ActivatedRoute } from '@angular/router';
import { IScript } from '../../../shared';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Rx';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ScriptDataService } from '../../../shared';
import { DialogsService } from '../../../shared/services/dialogs.service';

@Component({
  selector: 'app-scripts-list',
  templateUrl: './scripts-list.component.html'
})
export class ScriptsListComponent implements OnInit, OnDestroy {
  private querySubscription: Subscription;
  private scriptSubscription: Subscription;
  private scriptChangeSubscription: Subscription;
  public scripts: IScript[];
  public searchForm: FormGroup;
  public searchScriptname: string = '';
  public currentPage;

  constructor(
    private scriptDataService: ScriptDataService,
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private dialogsService: DialogsService
  ) { }

  ngOnInit() {
    this.getScripts();
    // everytime when a script changes refresh the scripts array
    this.scriptChangeSubscription = this.scriptDataService.scriptChange.subscribe((res: IScript) => {
      this.getScripts();
    });
    // get query parameter from active route
    this.querySubscription = this.route.queryParams.subscribe(
      (queryParam: any) => {
        if (queryParam['scriptname']) {
          this.searchScriptname = queryParam['scriptname'];
        }
      });
    this.initForm();
  }

  ngOnDestroy() {
    if (this.scriptSubscription) {
      this.scriptSubscription.unsubscribe();
    }
    if (this.scriptChangeSubscription) {
      this.scriptChangeSubscription.unsubscribe();
    }
    if (this.querySubscription) {
      this.querySubscription.unsubscribe();
    }
  }

  private initForm() {
    let scriptname = this.searchScriptname;

    this.searchForm = this.formBuilder.group({
      scriptname: [scriptname]
    });
  }

  getScripts() {
    this.scriptSubscription = this.scriptDataService.getScripts()
      .subscribe((res: IScript[]) => {
        this.scripts = res;
      });
  }

  onSearch() {
    this.searchScriptname = this.searchForm.value.scriptname;
    let queryParams: any = {};
    if (this.searchScriptname) { queryParams.scriptname = this.searchScriptname; }
    this.router.navigate(['/scripts'], { queryParams: queryParams });
  }

  onSearchClear() {
    this.searchForm.controls['scriptname'].setValue('');
    this.onSearch();
  }

  onDetails(id: string) {
    this.router.navigate(['/scripts', id], { queryParams: { returnUrl: this.router.url}});
  }

  onDelete(id: string) {
    this.scriptDataService.removeScript(parseInt(id, 10)).subscribe(
      () => {
        this.getScripts();
      });
  }

  onNew() {
    this.router.navigate(['/scripts', 'new'], { queryParams: { returnUrl: this.router.url}});
  }

  onEdit(scriptId: number) {
    this.router.navigate(['/scripts', scriptId, 'edit'], { queryParams: { returnUrl: this.router.url}});
  }

  openDialog(id: string) {
    this.dialogsService
      .confirm('Script delete', 'Are you sure you want to delete this script?')
      .subscribe(res => {
        if (res === true) {
          this.onDelete(id);
        }
      });
  }

}
