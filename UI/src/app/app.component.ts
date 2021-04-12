import { Component } from '@angular/core';
import { NavEmitterService } from './Services/NavEmitter/nav-emitter.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  providers: []
})
export class AppComponent {
  title = 'app';
  show = false;
  secondaryMinimized: boolean;
  secondaryNav = 'hidden';

  constructor(private navEmitter: NavEmitterService, private router: Router) {  

    this.navEmitter.toggleMainSideNav.subscribe((data: boolean) => {
      this.show = data;
    });
  }
}
