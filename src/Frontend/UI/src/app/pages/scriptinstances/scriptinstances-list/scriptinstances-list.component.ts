import { FormBuilder, FormGroup, Validators, FormControl } from '@angular/forms';
import { ActivatedRoute, Router, RouterStateSnapshot } from '@angular/router';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Rx';
import { IScriptInstance } from '../../../shared/interfaces/iscript-instance';
import {
  ScriptinstanceDataService
} from '../../../shared';

declare var moment: any;

@Component({
  selector: 'app-scriptinstances-list',
  templateUrl: './scriptinstances-list.component.html'
})
export class ScriptinstancesListComponent implements OnInit, OnDestroy {
  private subscription: Subscription;
  private querySubscription: Subscription;
  private minDate: String = '2017-02-03';
  private timeRegex: string = '(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]';
  public scriptInstances: IScriptInstance[];
  public searchScriptInstanceStatus: String[] = [];
  public searchScriptInstanceHostname: String = '';
  public searchScriptInstanceScriptname: String = '';
  public searchScriptInstanceDateFrom: String = '';
  public searchScriptInstanceDateTo: String = '';
  public searchScriptInstanceHourFrom: String = '';
  public searchScriptInstanceHourTo: String = '';
  public searchForm: FormGroup;
  public statusOptions = [
    { value: 'Created', label: 'Created' },
    { value: 'Running', label: 'Running' },
    { value: 'Completed', label: 'Completed' },
    { value: 'CompletedWithError', label: 'CompletedWithError' },
    { value: 'CompletedWithWarning', label: 'CompletedWithWarning' },
    { value: 'Aborted', label: 'Aborted' },
    { value: 'Timeout', label: 'Timeout' },
  ];
  public currentPage;


  constructor(
    private scriptinstanceDataService: ScriptinstanceDataService,
    private router: Router,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit() {
    this.getScriptInstances();
    this.querySubscription = this.route.queryParams.subscribe(
      (queryParam: any) => {
        if (queryParam['status']) {
          this.searchScriptInstanceStatus = queryParam['status'].split(',');
        }
        if (queryParam['hostname']) {
          this.searchScriptInstanceHostname = queryParam['hostname'];
        }
        if (queryParam['scriptname']) {
          this.searchScriptInstanceScriptname = queryParam['scriptname'];
        }
        if (queryParam['dateFrom']) {
          this.searchScriptInstanceDateFrom = queryParam['dateFrom'];
        }
        if (queryParam['dateTo']) {
          this.searchScriptInstanceDateTo = queryParam['dateTo'];
        }
        if (queryParam['hourFrom']) {
          this.searchScriptInstanceHourFrom = queryParam['hourFrom'];
        }
        if (queryParam['hourTo']) {
          this.searchScriptInstanceHourTo = queryParam['hourTo'];
        }
      });
    this.initForm();
  }
  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
    if (this.querySubscription) {
      this.querySubscription.unsubscribe();
    }
  }

  dateValidator(control: FormControl): { [s: string]: boolean } {
    if (moment(control.value, 'DD.MM.YYYY').isValid() || control.value === '') {
      return null;
    }
    return { date: true };
  }

  dateTimeToValidator(group: FormGroup): { [s: string]: boolean } {
    if (group.controls['dateFrom'].value === '' || group.controls['dateTo'].value === ''
      || (moment(group.controls['dateTo'].value + ' ' + group.controls['hourTo'].value, 'DD.MM.YYYY HH:mm')
        - moment(group.controls['dateFrom'].value + ' ' + group.controls['hourFrom'].value, 'DD.MM.YYYY HH:mm') >= 60000)) {
      return null;
    }
    return { date: true };
  }

  private initForm() {
    let status = this.searchScriptInstanceStatus;
    let hostname = this.searchScriptInstanceHostname;
    let scriptname = this.searchScriptInstanceScriptname;
    let dateFrom = this.searchScriptInstanceDateFrom;
    let dateTo = this.searchScriptInstanceDateTo;
    let hourFrom = this.searchScriptInstanceHourFrom;
    let hourTo = this.searchScriptInstanceHourFrom;
    if (hourFrom === '') {
      hourFrom = '00:00';
    }
    if (hourTo === '') {
      hourTo = '23:59';
    }

    this.searchForm = this.formBuilder.group({
      status: [status],
      hostname: [hostname],
      scriptname: [scriptname],
      dateTime: this.formBuilder.group({
        dateFrom: [dateFrom, this.dateValidator],
        dateTo: [dateTo, this.dateValidator],
        hourFrom: [hourFrom, Validators.pattern(this.timeRegex)],
        hourTo: [hourTo, Validators.pattern(this.timeRegex)]
      }, { validator: this.dateTimeToValidator })
    });
  }

  getScriptInstances() {
    this.subscription = this.scriptinstanceDataService.getScriptInstances()
      .subscribe((res: IScriptInstance[]) => {
        this.scriptInstances = res;
      });
  }

  onDetails(guid: String) {
    this.router.navigate(['/scriptinstances', guid], { queryParams: { returnUrl: this.router.url}});
  }

  onSearch() {
    this.searchScriptInstanceStatus = this.searchForm.value.status;
    this.searchScriptInstanceHostname = this.searchForm.value.hostname;
    this.searchScriptInstanceScriptname = this.searchForm.value.scriptname;
    this.searchScriptInstanceDateFrom = this.searchForm.value.dateTime.dateFrom;
    this.searchScriptInstanceDateTo = this.searchForm.value.dateTime.dateTo;
    this.searchScriptInstanceHourFrom = this.searchForm.value.dateTime.hourFrom;
    this.searchScriptInstanceHourTo = this.searchForm.value.dateTime.hourTo;
    let queryParams: any = {};
    if (this.searchScriptInstanceStatus.length > 0) { queryParams.status = this.searchScriptInstanceStatus.toString(); }
    if (this.searchScriptInstanceHostname) { queryParams.hostname = this.searchScriptInstanceHostname; }
    if (this.searchScriptInstanceScriptname) { queryParams.scriptname = this.searchScriptInstanceScriptname; }
    if (this.searchScriptInstanceDateFrom) { queryParams.dateFrom = this.searchScriptInstanceDateFrom; }
    if (this.searchScriptInstanceDateTo) { queryParams.dateTo = this.searchScriptInstanceDateTo; }
    if (this.searchScriptInstanceHourFrom !== '00:00') { queryParams.hourFrom = this.searchScriptInstanceHourFrom; }
    if (this.searchScriptInstanceHourTo !== '23:59') { queryParams.hourTo = this.searchScriptInstanceHourTo; }
    this.router.navigate(['/scriptinstances'], { queryParams: queryParams });
  }

  onSearchClear() {
    this.searchForm.controls['status'].setValue([]);
    this.searchForm.controls['hostname'].setValue('');
    this.searchForm.controls['scriptname'].setValue('');
    this.searchForm.controls['dateTime'].setValue({
      dateFrom: '',
      dateTo: '',
      hourFrom: '00:00',
      hourTo: '23:59'
    });
    this.onSearch();
  }

}
