import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { SharedModule } from './shared/shared.module';
import { LatestLogsComponent } from './pages/home/latest-logs/latest-logs.component';
import { LogStatisticsComponent } from './pages/home/log-statistics/log-statistics.component';
import { HomeComponent } from './pages/home/home.component';
import { MatCardModule, MatIconModule } from '@angular/material';
import { MomentModule } from 'angular2-moment';

@NgModule({
    imports: [
        SharedModule,
        RouterModule,
        MatIconModule,
        MatCardModule,
        MomentModule
    ],
    declarations: [
        LatestLogsComponent,
        LogStatisticsComponent,
        HomeComponent
    ]
})
export class CoreModule {}
