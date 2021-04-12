import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { EnrollStudentModel } from 'src/app/Models/enroll-student-model';

@Injectable({
  providedIn: 'root'
})
export class EnrollmentService {

  constructor(private http: HttpClient) { }

  /**
   * Get all students enrolled in a course
   * 
   * @param courseId id of course that students are enrolled in
   */
  getEnrolledStudents(courseId): Observable<any> {
    return this.http.get(`Enrollment/GetEnrolledStudents?courseId=${courseId}`).pipe(
    map(data => data),
    catchError(err => this.handleError(err)));
  }

  /**
   * Enroll students to a course
   * 
   * @param model enrollment model
   */
  enrollStudents(model: EnrollStudentModel): Observable<any> {
    return this.http.post(`Enrollment/EnrollStudents`, model).pipe(
    map(data => data),
    catchError(err => this.handleError(err)));
  }

  /**
   * Removes students from a course
   * 
   * @param studentId id of student being removed
   * @param courseId id of course that student is being enrolled to
   */
  removeStudentFromCourse(studentId: number, courseId: number): Observable<any> {
    return this.http.delete(`Enrollment/RemoveStudentFromCourse?studentId=${studentId}&courseId=${courseId}`).pipe(
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
