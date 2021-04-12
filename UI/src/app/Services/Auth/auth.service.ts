import { Injectable, Inject } from '@angular/core';
import { catchError, map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  /**
   * Validates authorization token
   * 
   * @param token token being authorized
   */
  validateToken(token: string): Observable<any> {
    return this.http.get(`Auth?accessToken=${token}`).pipe(
    map(data => data), // as AuthResponseModel),
    catchError(err => this.handleError(err)));
  }

  /**
   * Gets the authorization id
   */
  getAuthorizationId(): Observable<any> {
    return this.http.get("Auth/GetAuthorizationId").pipe(
    map(data => data), // as AuthResponseModel),
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
}
