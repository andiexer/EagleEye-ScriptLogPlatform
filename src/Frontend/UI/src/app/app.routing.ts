import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

// Components
import { HomeComponent } from './pages/home/home.component';
import { ErrorComponent } from './pages/error/error.component';

const routes: Routes = [
    // Root
    { path: '', component: HomeComponent},
    { path: 'scriptinstances', loadChildren: 'app/pages/scriptinstances/scriptinstances.module#ScriptinstancesModule' },
    { path: 'tenants', loadChildren: 'app/pages/tenants/tenants.module#TenantsModule' },
    { path: 'hosts', loadChildren: 'app/pages/hosts/hosts.module#HostsModule' },
    { path: 'scripts', loadChildren: 'app/pages/scripts/scripts.module#ScriptsModule' },
    { path: 'error', component: ErrorComponent }
];

export const routing: ModuleWithProviders = RouterModule.forRoot(routes);
