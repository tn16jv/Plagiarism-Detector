import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { AssignmentModel } from 'src/app/Models/assignment-model';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FileDownloadModel } from 'src/app/Models/file-download-model';

@Injectable({
  providedIn: 'root'
})
export class AssignmentSubmissionService {

  constructor(private http: HttpClient) { }

  /**
   * Gets all assignment submissions form an assignment posting
   * 
   * @param assignmentId id of assignment posting
   */
  getAssignmentSubmissions(assignmentId :Number): Observable<any> {    
    return this.http.get(`AssignmentSubmission/GetAssignmentSubmissions?assignmentId=${assignmentId}`).pipe(
    map(data => data),
    catchError(err => this.handleError(err)));
  }

  /**
   * Uploads an assignment to an assignment posting
   * 
   * @param formData assignment submission information
   * @param assignmentId id of assignment posting
   * @param courseId id of course that posting belongs to
   * @param fileType file type of assignment submission
   */
  uploadAssignment(formData : FormData, assignmentId: number, courseId: number, fileType: string): Observable<any> {      
    return this.http.post(`AssignmentSubmission/UploadAssignment?courseId=${courseId}&assId=${assignmentId}&filetype=${fileType}`, formData).pipe(
    map(data => data),
    catchError(err => this.handleError(err)));
  }

  /**
   * Downloads an assignment submission
   * 
   * 
   * @param assignmentId id of submission being downloaded
   * @param studentId id of student that the assignment belongs to
   */
  downloadAssignment(assignmentId: number, studentId: number = null): Observable<any> {
    let optional = "";
    if(studentId != null)
      optional = `&studentId=${studentId}`;

    return this.http.get(`AssignmentSubmission/DownloadAssignment?assignmentId=${assignmentId}${optional}`, { responseType: 'blob' });
  }

  /**
   * Deletes an assignment submission
   * 
   * @param assignmentId id of assignment being submitted
   * @param studentId id of student that assignment belongs to
   */
  deleteFile(assignmentId: number, studentId: number = null): Observable<FileDownloadModel> {
    let optional = "";
    if(studentId != null)
      optional = `&studentId=${studentId}`;

    return this.http.delete<FileDownloadModel>(`AssignmentSubmission/DeleteFile?assignmentId=${assignmentId}${optional}`);
  }

  /**
   * Gets the name of submission file
   * 
   * @param assignmentId id of assignment with filename that is requested
   * @param studentId id of student that assignment belongs to
   */
  getDownloadFileName(assignmentId: number, studentId: number = null): Observable<FileDownloadModel> {
    let optional = "";
    if(studentId != null)
      optional = `&studentId=${studentId}`;

    return this.http.get<FileDownloadModel>(`AssignmentSubmission/GetDownloadName?assignmentId=${assignmentId}${optional}`);
  }

  /**
   * Determines if the assignment was submitted late
   * 
   * @param assignmentId id of assignment being checked
   */
  IsPastDue(assignmentId: number): Observable<boolean> {
    return this.http.get<boolean>(`AssignmentSubmission/IsPastDue?assignmentId=${assignmentId}`);
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
