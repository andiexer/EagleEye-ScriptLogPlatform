import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, DoCheck, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs/Rx';
import { CustomValidators } from 'ng2-validation';
import { ILog, LogLevel, IScriptInstance, ScriptinstanceDataService } from '../../../shared';
import { ILogs } from '../../../shared/interfaces/ilogs';
import { PageEvent } from '@angular/material';

declare var moment: any;

@Component({
  selector: 'app-scriptinstance-details',
  templateUrl: './scriptinstance-details.component.html',
  styleUrls: ['scriptinstance-details.component.css']
})
export class ScriptinstanceDetailsComponent implements OnInit, OnDestroy, DoCheck {
  private subscription: Subscription;
  private idSubscription: Subscription;
  private logSubscription: Subscription;
  private querySubscription: Subscription;
  private scriptInstanceId: number;
  private returnUrl: string;
  public searchForm: FormGroup;
  public searchLogLevel: string[] = [];
  public searchText: string = '';
  public logLevelOptions = [
    { value: LogLevel.Fatal, label: 'Fatal' },
    { value: LogLevel.Error, label: 'Error' },
    { value: LogLevel.Warning, label: 'Warning' },
    { value: LogLevel.Info, label: 'Info' },
    { value: LogLevel.Debug, label: 'Debug' },
    { value: LogLevel.Trace, label: 'Trace' },
    { value: LogLevel.All, label: 'All' }
  ]
  public scriptInstance: IScriptInstance;
  public scriptInstanceLogs: ILog[];
  public length = 0;
  public pageSize = 10;
  public pageSizeOptions = [5, 10, 25, 100];
  public currentPage: number;
  public loadingScriptInstance: boolean;
  public loadingScriptInstanceLogs: boolean;

  constructor(private scriptinstanceDataService: ScriptinstanceDataService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private router: Router
  ) { }

  ngOnInit() {
    this.querySubscription = this.route.queryParams.subscribe(
      (queryParam: any) => {
        if (queryParam['logLevel']) {
          this.searchLogLevel = queryParam['logLevel'].split(',').map(item => parseInt(item, 10));
        }
        if (queryParam['text']) {
          this.searchText = queryParam['text'];
        }
        this.returnUrl = queryParam['returnUrl'] || '/scriptinstances';
      });
    this.idSubscription = this.route.params.subscribe(
      (params: any) => {
        this.scriptInstanceId = parseInt(params['guid'], 10);
        this.getScriptInstance();
        this.getScriptInstanceLogs();
      }
    );
    this.initForm();
  }

  ngOnDestroy() {
    if (this.subscription) { this.subscription.unsubscribe(); }
    if (this.querySubscription) { this.querySubscription.unsubscribe(); }
    if (this.idSubscription) { this.idSubscription.unsubscribe(); }
    if (this.logSubscription) { this.logSubscription.unsubscribe(); }
  }

  ngDoCheck() {
    if (this.scriptInstance) {
      if (this.scriptInstance.instanceStatus !== 'Running' && this.scriptInstance.instanceStatus !== 'Created') {
        if (this.subscription) {
          this.subscription.unsubscribe();
        }
        if (this.logSubscription && this.loadingScriptInstanceLogs) {
          this.logSubscription.unsubscribe();
        }
      }
    }
  }

  private initForm() {
    let logLevel = this.searchLogLevel;
    let text = this.searchText;

    this.searchForm = this.formBuilder.group({
      logLevel: [logLevel],
      text: [text]
    });
  }

  getScriptInstance() {
    this.loadingScriptInstance = true;
    this.subscription = this.scriptinstanceDataService.getScriptInstance(this.scriptInstanceId)
      .subscribe((res: IScriptInstance) => {
        this.loadingScriptInstance = false;
        this.scriptInstance = res;
      });
  }

  getScriptInstanceLogs() {
    this.loadingScriptInstanceLogs = true;
    if (this.logSubscription) { this.logSubscription.unsubscribe(); }
    this.logSubscription = this.scriptinstanceDataService.getScriptInstanceLogs(
      this.scriptInstanceId,
      this.searchLogLevel,
      this.searchText,
      this.currentPage + 1,
      this.pageSize
    ).subscribe((res: ILogs) => {
      this.loadingScriptInstanceLogs = false;
      this.scriptInstanceLogs = res.logs;
      this.currentPage = res.pagination.CurrentPage - 1;
      this.pageSize = res.pagination.ItemsPerPage;
      this.length = res.pagination.TotalItems;
    });
  }

  onSearch() {
    this.searchLogLevel = this.searchForm.value.logLevel;
    this.searchText = this.searchForm.value.text;
    let queryParams: any = {};
    if (this.searchLogLevel.length > 0) { queryParams.logLevel = this.searchLogLevel.toString(); }
    if (this.searchText) { queryParams.text = this.searchText; }
    if (this.returnUrl) { queryParams.returnUrl = this.returnUrl; }
    this.router.navigate(['/scriptinstances', this.scriptInstanceId], { queryParams: queryParams });
    this.currentPage = 0;
    this.getScriptInstanceLogs();
  }

  onSearchClear() {
    this.searchForm.controls['text'].setValue('');
    this.searchForm.controls['logLevel'].setValue([]);
    this.onSearch();
  }

  goBack() {
    this.router.navigateByUrl(this.returnUrl);
  }

  onPageChange(pageEvent: PageEvent) {
    this.currentPage = pageEvent.pageIndex;
    this.pageSize = pageEvent.pageSize;
    this.getScriptInstanceLogs();
  }

}
