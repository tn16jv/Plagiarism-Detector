import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { UrlModel } from '../../Models/url-model';
import { AuthorizationService } from '../../Services/Authorization/authorization.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private authorizationService: AuthorizationService) {
    
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    // Check for authorization before proceeding
    let authorizationIdReq = null;
    if (next.data && next.data.authorizationIdReq) authorizationIdReq = next.data.authorizationIdReq;
    if (!this.isAuthorized(authorizationIdReq)) {
      alert("Incorrect authorization access level. Please try navigate to the Home Page and try again. Thank you");
      this.router.navigate([`/`]);
    }


    if (environment.AuthEnabled) { // if authentication is active
      if (localStorage.getItem('app_token')) {
        if (!this.tokenExpired()) { // if token is NOT expired
          return true;
        }
      }
      // Otherwise, token must be expired or null. Redirect user to STS login page
      this.getToken(next);
      return false;
    } else {
      return true;
    }
  }


  // returns true if the token is expired, false otherwise
  tokenExpired(): boolean {
    const expireDate = (Number)(localStorage.getItem('expire_date'));
    const now = Date.now().valueOf();
    if (now >= expireDate) {
      return true;
    }
    return false;
  }

  isAuthorized(authorizationIdReq: number): boolean {
    if (authorizationIdReq == null) return true;
    let authorizationRoleId = 0;
    this.authorizationService.getAuthorizationId().subscribe((x:number) => {
      authorizationRoleId = x;
    });

    // If System Admin role required
    if (authorizationIdReq == 2 && authorizationRoleId == 2) return true;

    // If Operations role required
    else if (authorizationIdReq == 1 && (authorizationRoleId == 1 || authorizationRoleId == 2)) return true;

    // If Group Coordinator role required
    else if (authorizationIdReq == 4 && (authorizationRoleId == 1 || authorizationRoleId == 2 || authorizationRoleId == 4)) return true;

    // If Developer role required
    else if (authorizationIdReq == 3 && (authorizationRoleId == 1 || authorizationRoleId == 2 || authorizationRoleId == 4 || authorizationRoleId == 3)) return true;
    
    else
      return false;
  }

  getToken(routeSnapshot: ActivatedRouteSnapshot) {
    const parameters = routeSnapshot.queryParams;
    const url = routeSnapshot.url.join('/');
    const urlObject: UrlModel = {
      URL: url,
      QueryParams: parameters,
    };

    localStorage.setItem('currentPage', JSON.stringify(urlObject));
    window.location.href = `${environment.StsURL}/oauth2/authorize?response_type=token&scope=openid` +
      `&client_id=${environment.ClientId}&resource=${environment.Resource}&redirect_uri=${environment.RedirectURI}`;
  }

}
