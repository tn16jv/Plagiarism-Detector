import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { reject } from 'q';
import { AuthorizationService } from 'src/app/Services/Authorization/authorization.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(private authorizationService: AuthorizationService) { }

  //Determines if the user is an admin
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      return this.authorizationService.getAuthorizationId().pipe(
        map(e => {
          if(e == 2) {
            return true; // admin
          } 
          else {
            reject();
            return false;
          }
        }), catchError(() => {
          reject();
          return of(false);          
        }));
  }
  
}
