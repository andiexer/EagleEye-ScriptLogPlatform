import { Component, OnDestroy, OnInit } from '@angular/core';
import { LogDataService } from '../../../shared';
import { Subscription } from 'rxjs/Rx';

@Component({
  selector: 'app-log-statistics',
  templateUrl: './log-statistics.component.html'
})
export class LogStatisticsComponent implements OnInit, OnDestroy {
  totalLogs: number;
  totalByLogLevel: {[loglevel: string]: string; } = {};
  private totalLogsSubscription: Subscription;
  private totalLogsByLogLevelSubscription: Subscription;
  subscriptionActive: boolean;


  constructor(private logDataService: LogDataService) { }

  ngOnInit() {
    this.getLogsTotalAmount();
    this.getLogsTotalByLoglevel();
  }

  ngOnDestroy() {
    if (this.totalLogsSubscription) { this.totalLogsSubscription.unsubscribe(); }
    if (this.totalLogsByLogLevelSubscription) { this.totalLogsByLogLevelSubscription.unsubscribe(); }
  }

  getLogsTotalAmount() {
    this.totalLogsSubscription = this.logDataService.getTotalLogs().subscribe(data  => this.totalLogs = data.totalLogs);
    this.subscriptionActive = true;
  }

  updateSubscription() {
    if (this.subscriptionActive) {
      console.log('closing, try to unsubscribe subscription');
      this.totalLogsSubscription.unsubscribe();
      this.subscriptionActive = false;
    } else {
      console.log('add subscription for totallogs');
      this.getLogsTotalAmount();
      this.subscriptionActive = true;
    }
  }

  getLogsTotalByLoglevel() {
    this.totalLogsByLogLevelSubscription = this.logDataService.getTotalLogsByLoglevel().subscribe((res: any) => {
      for (let loglevel of res) {
          this.totalByLogLevel[loglevel.logLevel] = loglevel.amount;
      }
    });
  }
}
