import { DatePipe } from '@angular/common/src/pipes/date_pipe';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderByPipe } from './';
import { ConfirmDialogComponent } from './component/confirm-dialog/confirm-dialog.component';
import { MomentModule } from 'angular2-moment';
import { LogLevelIconPipe } from './pipes/log-level-icon.pipe';
import { MatIconModule, MatDialogModule, MatButtonModule } from '@angular/material';

@NgModule({
    imports: [
        CommonModule,
        FlexLayoutModule,
        MatIconModule,
        MatDialogModule,
        MatButtonModule
    ],
    declarations: [
        OrderByPipe,
        ConfirmDialogComponent,
        LogLevelIconPipe
    ],
    exports: [
        CommonModule,
        ReactiveFormsModule,
        OrderByPipe,
        FlexLayoutModule,
        ConfirmDialogComponent,
        LogLevelIconPipe,
    ],
    entryComponents: [
        ConfirmDialogComponent
    ]
})
export class SharedModule {}
