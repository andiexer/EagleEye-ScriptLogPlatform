import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs/Rx';

import { ILog, LogLevel, LogDataService } from '../../../shared';

@Component({
  selector: 'app-latest-logs',
  templateUrl: './latest-logs.component.html'
})
export class LatestLogsComponent implements OnInit, OnDestroy {
  @Input() totalItems: number = 10;
  private subscription: Subscription;
  public displayedColumns = ['logDateTime', 'logLevel', 'logText', 'scriptInstance'];
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
  public latestLogs: ILog[];

  constructor(
    private logDataService: LogDataService
  ) { }

  ngOnInit() {
    this.getLogs(this.totalItems);
  }
  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  getLogs(totalItems: number) {
    this.subscription = this.logDataService.getLatestLogs(totalItems)
      .subscribe((res: ILog[]) => {
        this.latestLogs = res;
      });
  }

}
