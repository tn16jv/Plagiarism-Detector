import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, observable, of } from 'rxjs';
import { AuthorizationService } from 'src/app/Services/Authorization/authorization.service';
import { map, catchError } from 'rxjs/operators';
import { reject } from 'q';

@Injectable({
  providedIn: 'root'
})
export class ProfessorGuard implements CanActivate {

  /**
   * Constrcuts a Professor guard to authorize professors
   * 
   * @param authorizationService service for authorizing professors
   */
  constructor(private authorizationService: AuthorizationService) { }

  //Determines if the user is an admin or a professor
  canActivate(
    next: ActivatedRouteSnapshot, state: RouterStateSnapshot)
    : Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    return this.authorizationService.getAuthorizationId().pipe(
      map(e => {
        if(e == 1 || e == 2) {
          return true; // admin or prof
        } 
        else 
          reject();
      }), catchError(() => {
        reject();
        return of(false);        
      }));
  }

}
