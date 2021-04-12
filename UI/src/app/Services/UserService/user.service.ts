import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { UserModel } from 'src/app/Models/user-model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  //Gets the current logged in user from the data base
  getUser(): Observable<any> {
    return this.http.get(`User/GetUser`).pipe(
    map(data => data),
    catchError(err => this.handleError(err)));
  }

  /**
   * Gets user by id
   * 
   * @param userId id of user being retrieved
   */
  getUserById(userId: number): Observable<any> {
    return this.http.get(`User/GetUser?userId=${userId}`).pipe(
    map(data => data),
    catchError(err => this.handleError(err)));
  }

  //Gets all users in the database
  getUsers(): Observable<UserModel[]> {
    return this.http.get<UserModel[]>(`User/GetUsers`);
  }

  /**
   * Updates the user in the database
   * 
   * @param user updated user
   */
  updateUser(user: UserModel): Observable<any> {    
    return this.http.put(`User/UpdateUser`, user).pipe(
    map(data => data),
    catchError(err => this.handleError(err)));
  }
  
  /**
   * Update to the user made by an admin
   * 
   * @param user updated user
   */
  adminUpdateUser(user): Observable<any> {    
    return this.http.put(`User/AdminUpdateUser`, user).pipe(
    map(data => data),
    catchError(err => this.handleError(err)));
  }

  /**
   * Adds a user to the database
   * 
   * @param user user being added
   */
  addUser(user): Observable<any> {    
    return this.http.post(`User/AddUser`, user).pipe(
    map(data => data),
    catchError(err => this.handleError(err)));
  }

  /**
   * Handles errors in the assignemnt service
   * 
   * @param error response being handled
   */
  private handleError(error: Response | any) {
    return null;
  }
}
