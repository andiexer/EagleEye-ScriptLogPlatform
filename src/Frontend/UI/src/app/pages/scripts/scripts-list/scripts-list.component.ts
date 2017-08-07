import { isNullOrUndefined } from 'util';
import { Router, ActivatedRoute } from '@angular/router';
import { IScript, IScripts } from '../../../shared';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Rx';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ScriptDataService } from '../../../shared';
import { DialogsService } from '../../../shared/services/dialogs.service';
import { PageEvent } from '@angular/material';
import { IScriptEdit } from '../../../shared/interfaces/iscript-edit';

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
  public length = 0;
  public pageSize = 10;
  public pageSizeOptions = [5, 10, 25, 100];
  public currentPage: number;
  public loadingScripts: boolean;

  constructor(
    private scriptDataService: ScriptDataService,
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private dialogsService: DialogsService
  ) { }

  ngOnInit() {
    // everytime when a script changes refresh the scripts array
    this.scriptChangeSubscription = this.scriptDataService.scriptChange.subscribe((res: IScriptEdit) => {
      this.getScripts();
    });
    // get query parameter from active route
    this.querySubscription = this.route.queryParams.subscribe(
      (queryParam: any) => {
        if (queryParam['scriptname']) {
          this.searchScriptname = queryParam['scriptname'];
        }
      });
    this.getScripts();
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
    this.loadingScripts = true;
    this.scriptSubscription = this.scriptDataService.getScripts(this.searchScriptname, this.currentPage + 1, this.pageSize)
      .subscribe((res: IScripts) => {
        this.loadingScripts = false;
        this.scripts = res.scripts;
        this.currentPage = res.pagination.CurrentPage - 1;
        this.pageSize = res.pagination.ItemsPerPage;
        this.length = res.pagination.TotalItems;
      },
      error => {
        console.log(error);
      });
  }

  onSearch() {
    this.searchScriptname = this.searchForm.value.scriptname;
    let queryParams: any = {};
    if (this.searchScriptname) { queryParams.scriptname = this.searchScriptname; }
    this.router.navigate(['/scripts'], { queryParams: queryParams });
    this.getScripts();
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

  onPageChange(pageEvent: PageEvent) {
    this.currentPage = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.getScripts();
  }

}
