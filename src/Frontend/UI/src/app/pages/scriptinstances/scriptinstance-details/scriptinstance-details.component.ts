import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, DoCheck, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Subscription } from 'rxjs/Rx';
import { CustomValidators } from 'ng2-validation';
import { ILog, LogLevel, IScriptInstance, ScriptinstanceDataService } from '../../../shared';

declare var moment: any;

@Component({
  selector: 'app-scriptinstance-details',
  templateUrl: './scriptinstance-details.component.html',
  styleUrls: ['scriptinstance-details.component.css']
})
export class ScriptinstanceDetailsComponent implements OnInit, OnDestroy, DoCheck {
  private subscription: Subscription;
  private guidSubscription: Subscription;
  private logSubscription: Subscription;
  private querySubscription: Subscription;
  private scriptInstanceGuid: String;
  private returnUrl: string;
  public itemsPerPage: Number = 10;
  public searchForm: FormGroup;
  public searchLogLevel: LogLevel[];
  public searchText: String = '';
  public logLevel = LogLevel;
  public logLevelIcon: String[] = [
    'error',
    'error',
    'warning',
    'info',
    'info',
    'info',
    'info'
  ];
  public logLevelOptions = [
    {value: LogLevel.Fatal, label: 'Fatal'},
    {value: LogLevel.Error, label: 'Error'},
    {value: LogLevel.Warning, label: 'Warning'},
    {value: LogLevel.Info, label: 'Info'},
    {value: LogLevel.Debug, label: 'Debug'},
    {value: LogLevel.Trace, label: 'Trace'},
    {value: LogLevel.All, label: 'All'}
  ]
  public scriptInstance: IScriptInstance;
  public scriptInstanceLogs: ILog[];
  public currentPage;

  constructor(private scriptinstanceDataService: ScriptinstanceDataService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private router: Router
  ) { }

  ngOnInit() {
    this.guidSubscription = this.route.params.subscribe(
      (params: any) => {
        this.scriptInstanceGuid = params['guid'];
        this.getScriptInstance(this.scriptInstanceGuid.toString());
        this.getScriptInstanceLogs(this.scriptInstanceGuid.toString());
      }
    );
    this.querySubscription = this.route.queryParams.subscribe(
      (queryParam: any) => {
        if (queryParam['itemsPerPage']) {
          this.itemsPerPage = queryParam['itemsPerPage'];
        }
        if (queryParam['logLevel']) {
          this.searchLogLevel = queryParam['logLevel'].split(',').map(item => parseInt(item, 10));
        }
        if (queryParam['text']) {
          this.searchText = queryParam['text'];
        }
        this.returnUrl = queryParam['returnUrl'] || '/scriptinstances';
      });
    this.initForm();
  }

  ngOnDestroy() {
    if (this.subscription) { this.subscription.unsubscribe(); }
    if (this.querySubscription) { this.querySubscription.unsubscribe(); }
    if (this.guidSubscription) { this.guidSubscription.unsubscribe(); }
    if (this.logSubscription) { this.logSubscription.unsubscribe(); }
  }

  ngDoCheck() {
    if (this.scriptInstance) {
      if (this.scriptInstance.instanceStatus !== 'Running' && this.scriptInstance.instanceStatus !== 'Created') {
        if (this.subscription) {
          this.subscription.unsubscribe();
        }
        if (this.logSubscription && this.scriptInstanceLogs) {
          this.logSubscription.unsubscribe();
        }
      }
    }
  }

  private initForm() {
    let itemsPerPage = this.itemsPerPage;
    let logLevel = this.searchLogLevel;
    let text = this.searchText;

    this.searchForm = this.formBuilder.group({
      itemsPerPage: [itemsPerPage, [CustomValidators.range([10, 100])]],
      logLevel: [logLevel],
      text: [text]
    });
  }

  getScriptInstance(guid: string) {
    this.subscription = this.scriptinstanceDataService.getScriptInstance(guid)
      .subscribe((res: IScriptInstance) => {
        this.scriptInstance = res;
      });
  }

  getScriptInstanceLogs(guid: string) {
    this.logSubscription = this.scriptinstanceDataService.getScriptInstanceLogs(guid)
      .subscribe((res: ILog[]) => {
        this.scriptInstanceLogs = res;
      });
  }

  onSearch() {
    this.itemsPerPage = this.searchForm.value.itemsPerPage;
    this.searchLogLevel = this.searchForm.value.logLevel;
    this.searchText = this.searchForm.value.text;
    let queryParams: any = {};
    if (this.itemsPerPage !== 10) { queryParams.itemsPerPage = this.itemsPerPage; }
    if (this.searchLogLevel.length > 0) { queryParams.logLevel = this.searchLogLevel.toString(); }
    if (this.searchText) { queryParams.text = this.searchText; }
    if (this.returnUrl) { queryParams.returnUrl = this.returnUrl; }
    this.router.navigate(['/scriptinstances', this.scriptInstanceGuid], { queryParams: queryParams });
  }

  onSearchClear() {
    this.searchForm.controls['itemsPerPage'].setValue(10);
    this.searchForm.controls['logLevel'].setValue([]);
    this.searchForm.controls['text'].setValue('');
    this.onSearch();
  }

  goBack() {
    this.router.navigateByUrl(this.returnUrl);
  }

}
