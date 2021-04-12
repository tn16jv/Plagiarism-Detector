import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { CourseModel } from 'src/app/Models/course-model';

@Injectable({
  providedIn: 'root'
})
export class CourseService {

  constructor(private http: HttpClient) { }

  //Gets all courses that the professor is enrolled in
  getCourses(): Observable<any> {
    var userId = localStorage.getItem('user_id');
    return this.http.get(`Course/GetCourses?profId=${userId}`).pipe(
    map(data => data),
    catchError(err => this.handleError(err)));
  }

  /**
   * Gets a course based on id
   * 
   * @param courseId id of course being retrieved
   */
  getCourse(courseId: Number): Observable<any> {
    return this.http.get(`Course/GetCourse?courseId=${courseId}`).pipe(
      map(data => data),
      catchError(err => this.handleError(err)));
  }

  //Gets all the courses that a student in enrolled in
  getStudentCourses(): Observable<any> {
    var userId = localStorage.getItem('user_id');
    return this.http.get(`Course/GetStudentCourses?userId=${userId}`).pipe(
    map(data => data),
    catchError(err => this.handleError(err)));
  }

  /**
   * Gets the information of a course
   * 
   * @param courseId id of course getting info retrieved
   */
  getCourseInfo(courseId): Observable<any> {
    return this.http.get(`Course/GetCourseInfo?courseId=${courseId}`).pipe(
      map(data => data),
      catchError(err => this.handleError(err)));
  }
  
  /**
   * Adds a course to the database
   * 
   * @param course course being added
   */
  addCourse(course: CourseModel): Observable<any> {
    course.profId = parseInt(localStorage.getItem('user_id'));
    return this.http.put(`Course/AddCourse`, course).pipe(
    map(data => data),
    catchError(err => this.handleError(err)));
  }

  /**
   * Updates a course in the database
   * 
   * @param course updated course
   */
  updateCourse(course: CourseModel): Observable<any> {    
    return this.http.put(`Course/UpdateCourse`, course).pipe(
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
