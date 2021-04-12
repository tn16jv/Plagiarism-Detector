import { Injectable, Inject, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {

  constructor(private http: HttpClient) { }

  /**
   * Gets the authorization id
   */
  public getAuthorizationId(): Observable<any> {
    return this.http.get("Auth/GetAuthorizationId").pipe(
      map(data => this.storeId(data)),
      catchError(err => this.handleError(err)));
  }

  /**
   * Handles errors in the assignemnt service
   * 
   * @param error response of error
   */
  private handleError(error: Response | any) {
    return null;
  }

  /**
   * Stores authorization id in local storage
   * 
   * @param data authorization id
   */
  storeId(data): object {
    if (data != null && data != undefined)
      localStorage.setItem('auth_id', data.toString());
    return data;
  }

}
