import { Component } from '@angular/core';
import { ConfigService } from './shared';
import {
  Router,
  // import as RouterEvent to avoid confusion with the DOM Event
  Event as RouterEvent,
  NavigationStart,
  NavigationEnd,
  NavigationCancel,
  NavigationError
} from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  // styleUrls: ['./app.component.css']
})
export class AppComponent {
  navItems = [
    { name: 'Home', route: '', exact: true },
    { name: 'Scriptinstances', route: 'scriptinstances', exact: false },
    { name: 'Scripts', route: 'scripts', exact: false },
    { name: 'Hosts', route: 'hosts', exact: false },
  ];
  loading: boolean = true;

  constructor(
    public configService: ConfigService,
    private router: Router
  ) {
    router.events.subscribe((event: RouterEvent) => {
      this.navigationInterceptor(event);
    });
  }

  // Shows and hides the loading spinner during RouterEvent changes
  navigationInterceptor(event: RouterEvent): void {
    if (event instanceof NavigationStart) {
      this.loading = true;
    }
    if (event instanceof NavigationEnd) {
      this.loading = false;
    }

    // Set loading state to false in both of the below events to hide the spinner in case a request fails
    if (event instanceof NavigationCancel) {
      this.loading = false;
    }
    if (event instanceof NavigationError) {
      this.loading = false;
    }
  }

}
