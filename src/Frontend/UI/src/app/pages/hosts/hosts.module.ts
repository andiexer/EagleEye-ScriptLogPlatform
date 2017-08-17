import { NgModule } from '@angular/core';
import { HostsComponent } from './hosts.component';
import { HostsListComponent } from './hosts-list/hosts-list.component';
import { HostDetailComponent } from './host-detail/host-detail.component';
import { HostDetailResolver } from './host-detail/host-detail.resolver';
import { HostEditComponent } from './host-edit/host-edit.component';
import { HostEditResolver } from './host-edit/host-edit.resolver';
import { HostEditGuard } from './host-edit/host-edit.guard';
import { hostRouting } from './hosts.routing';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
  imports: [
    SharedModule,
    hostRouting
  ],
  declarations: [
    HostsComponent,
    HostsListComponent,
    HostDetailComponent,
    HostEditComponent
  ],
  providers: [
    HostDetailResolver,
    HostEditResolver,
    HostEditGuard
  ]
})
export class HostsModule { }
