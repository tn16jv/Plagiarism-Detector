import { Component, OnInit, Input } from '@angular/core';
import { NavEmitterService } from '../../Services/NavEmitter/nav-emitter.service';
import { AuthorizationService } from '../../Services/Authorization/authorization.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-env-side-nav',
  templateUrl: './env-side-nav.component.html',
  styleUrls: ['./env-side-nav.component.scss']
})
export class EnvSideNavComponent implements OnInit {
  
  public authorizationRoleId: number; //role of user accessing site

  @Input() show;
  selectedTab = "home";

  /**
   * Constructs the EnvSideNav component for navigation around the site
   * 
   * @param authorizationService service for checking user role
   * @param router router for navigation
   */
  constructor(private authorizationService: AuthorizationService, private router: Router) {
    this.getAuthId();

    this.router.events.pipe(
    ).subscribe(() => {
      this.getAuthId();
    });
  }

  //Gets user role on initialization 
  ngOnInit() {
    this.getAuthId();
  }

  //Gets the role of user accessing the site
  getAuthId() {
    let authId = localStorage.getItem('auth_id');
    if (authId === null) {
      this.authorizationService.getAuthorizationId().subscribe((x: number) => {
        if (x != null)
          this.authorizationRoleId = x;
      });
    }
    else {
      this.authorizationRoleId = Number.parseInt(authId);
    }
  }

  onGoToHome() {}
}
