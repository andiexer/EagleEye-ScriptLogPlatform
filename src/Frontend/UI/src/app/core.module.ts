import { NgModule } from '@angular/core';
import { MaterialModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { SharedModule } from './shared/shared.module';
import { LatestLogsComponent } from './pages/home/latest-logs/latest-logs.component';
import { LogStatisticsComponent } from './pages/home/log-statistics/log-statistics.component';
import { HomeComponent } from './pages/home/home.component';

@NgModule({
    imports: [
        SharedModule,
        RouterModule
    ],
    declarations: [
        LatestLogsComponent,
        LogStatisticsComponent,
        HomeComponent
    ]
})
export class CoreModule {}
