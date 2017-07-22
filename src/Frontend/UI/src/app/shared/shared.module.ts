import { DatePipe } from '@angular/common/src/pipes/date_pipe';
import { MaterialModule } from '@angular/material';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Ng2PaginationModule } from 'ng2-pagination';
import { OrderByPipe } from './';
import { PaginationComponent } from './component/pagination/pagination.component';
import { ConfirmDialogComponent } from './component/confirm-dialog/confirm-dialog.component';
import { MomentModule } from 'angular2-moment';

@NgModule({
    imports: [
        Ng2PaginationModule,
        CommonModule,
        MaterialModule,
        FlexLayoutModule,
        MomentModule
    ],
    declarations: [
        OrderByPipe,
        PaginationComponent,
        ConfirmDialogComponent
    ],
    exports: [
        CommonModule,
        Ng2PaginationModule,
        ReactiveFormsModule,
        OrderByPipe,
        PaginationComponent,
        MaterialModule,
        FlexLayoutModule,
        ConfirmDialogComponent,
        MomentModule
    ],
    entryComponents: [
        ConfirmDialogComponent
    ]
})
export class SharedModule {}
