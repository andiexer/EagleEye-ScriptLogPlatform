import { Component } from '@angular/core';
import { ConfigService } from './shared';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  // styleUrls: ['./app.component.css']
})
export class AppComponent {
  navItems = [
    {name: 'Home', route: '', exact: true},
    {name: 'Scriptinstances', route: 'scriptinstances', exact: false},
    {name: 'Scripts', route: 'scripts', exact: false},
    {name: 'Hosts', route: 'hosts', exact: false},
  ];

  constructor(
    public configService: ConfigService
  ) { }

}
