import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { FileByLineModel, ComparisonResults } from 'src/app/Models/comparison-model';

@Injectable({
  providedIn: 'root'
})
export class ComparisonService {

  constructor(private http: HttpClient) { }

  /**
   * Compares all submissions from an assignment posting
   * 
   * @param assignmentId id of assignment posting
   */
  compareAssignments(assignmentId: number): Observable<any> {
    return this.http.get<object>(`Plagiarism/RepoCompare?assignId=${assignmentId}`);
  }

  /**
   * Compares a specific set of assignments from an assignment posting
   * 
   * @param assignmentId if of assignment posting
   * @param studentIds ids of students that own assignments being compared
   */
  compareSpecificAssignments(assignmentId: number, studentIds: number[]): Observable<string> {
    // this is probably broken
    return this.http.get<string>(`Plagiarism/MultiAssignCompare?assignId=${assignmentId}&userIds=${studentIds}`);
  }

  /**
   * Compares a test suite of assignments
   * 
   * @param fileType type of files being compared
   * @param formData test suite of assignments
   */
  test(fileType: string, formData: FormData): Observable<any> {
    return this.http.post<object>(`PlagiarismTest/UploadTest?fileType=${fileType}`, formData); // for some reason I have to make this an object...
  }

  //Gets the results of the test suite comparison
  getTestData(): Observable<FileByLineModel[]> {
    return this.http.get<FileByLineModel[]>(`Plagiarism/TemporaryTest`);
  }

  /**
   * Gets the comparison results
   * 
   * @param fileId id of file that is having its results retrieved 
   */
  getCompare(fileId: string): Observable<ComparisonResults> {
    return this.http.get<ComparisonResults>(`Plagiarism/GetCompareResults?guid=${fileId}`);
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
