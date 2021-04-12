import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { AssignmentModel } from 'src/app/Models/assignment-model';
import { Router } from '@angular/router';
import { AssignmentInfoModel } from 'src/app/Models/assignment-info-model';

@Injectable({
  providedIn: 'root'
})
export class AssignmentService {

  
  constructor(private http: HttpClient, private router: Router) { }

  /**
   * Gets assignments postings for a course
   * 
   * @param courseId id of course that postings belong to 
   * @param authId role of user accessing assignment postings
   */
  getAssignments(courseId: Number, authId: Number): Observable<any> {
    let option = authId == 3 ? "ForStudent" : ""; // if user is a student

    return this.http.get(`Assignment/GetAssignments${option}?courseId=${courseId}`).pipe(
    map(data => data),
    catchError(err => this.handleError(err)));
  }

  /**
   * Gets an assignment posting for a course
   * 
   * @param courseId id of course that postings belong to 
   * @param assignmentId id of posting being retreived
   */
  getAssignment(courseId: Number, assignmentId: Number): Observable<any> {    

    return this.http.get(`Assignment/GetAssignment?courseId=${courseId}&assignmentId=${assignmentId}`).pipe(
    map(data => data),
    catchError(err => this.handleError(err)));
  }

  /**
   * Adds an assignment posting to a course
   * 
   * @param assignment assignment posting being added
   */
  addAssignment(assignment: AssignmentModel): Observable<any> {    
    return this.http.post(`Assignment/AddAssignment`, assignment).pipe(
    map(data => data),
    catchError(err => this.handleError(err)));
  }

  /**
   * Updates an assignment posting
   * 
   * @param assignment updated assignment posting
   */
  updateAssignment(assignment: AssignmentModel): Observable<any> {    
    return this.http.put(`Assignment/UpdateAssignment`, assignment).pipe(
    map(data => data),
    catchError(err => this.handleError(err)));
  }

  /**
   * Gets the information of an assignment posting
   * 
   * @param assignmentId id of posting with required information
   */
  getAssignmentInfo(assignmentId: number): Observable<AssignmentInfoModel> {    
    return this.http.get<AssignmentInfoModel>(`Assignment/GetAssignmentInfo?assignmentId=${assignmentId}`);
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
