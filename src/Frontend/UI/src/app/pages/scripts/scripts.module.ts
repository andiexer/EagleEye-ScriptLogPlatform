import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ScriptsComponent } from './scripts.component';
import { ScriptsListComponent } from './scripts-list/scripts-list.component';
import { ScriptDetailComponent } from './script-detail/script-detail.component';
import { ScriptDetailResolver } from './script-detail/script-detail.resolver';
import { ScriptEditGuard } from './script-edit/script-edit.guard';
import { ScriptEditComponent } from './script-edit/script-edit.component';
import { scriptRouting } from './scripts.routing';
import { SharedModule } from '../../shared/shared.module';
import { MatIconModule, MatCardModule, MatButtonModule, MatPaginatorModule, MatInputModule } from '@angular/material';
import { MomentModule } from 'angular2-moment';

@NgModule({
  imports: [
    scriptRouting,
    SharedModule,
    MatIconModule,
    MatCardModule,
    MatButtonModule,
    MatPaginatorModule,
    MatInputModule,
    MomentModule
  ],
  declarations: [
    ScriptsComponent,
    ScriptsListComponent,
    ScriptDetailComponent,
    ScriptEditComponent
  ],
  providers: [
    ScriptDetailResolver,
    ScriptEditGuard
  ]
})
export class ScriptsModule { }
