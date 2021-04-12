import { Component, OnInit } from '@angular/core';
import { AssignmentSubmissionService } from 'src/app/Services/AssignmentSubmission/assignment-submission.service';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorizationService } from 'src/app/Services/Authorization/authorization.service';
import { Location } from '@angular/common';

@Component({
  selector: 'app-submit-assignment',
  templateUrl: './submit-assignment.component.html',
  styleUrls: ['./submit-assignment.component.scss']
})
export class SubmitAssignmentComponent implements OnInit {

  assignmentId;                     //id of assignment posting
  courseId;                         //id of course that posting belongs to
  fileType: string;                 //type of file being submitted
  fileSelector: HTMLElement;        //file selector
  fileTypes = ["cpp", "java", "c"]; //types of files that can be submitted
  authorizationRoleId: number;      //role of user viewing page  
  hasDownload = true;               //required for download-assignment component (destroy if no download found)
  pastDue = false;                  //if the assignment is late

  /**
   * Constructs the SubmitAssignment component for submissions to assignment postings
   * 
   * @param submissionService service for submiting assignments to postings
   * @param authorizationService service for getting user role
   * @param aRoute activated route
   * @param router router for navigation
   */
  constructor(private submissionService: AssignmentSubmissionService, private authorizationService: AuthorizationService, 
    private aRoute: ActivatedRoute, private router: Router) { }

  //Gets course and assign ids and adds event listener to file selector on initialization
  ngOnInit() {
    this.aRoute.params.subscribe(params => {
      this.courseId = params['courseId'];
      this.assignmentId = params['assignmentId'];

      this.fileSelector = document.getElementById('fileSelector');
      this.fileSelector.addEventListener('change', (event) => {
        this.invalid("files");
      });

      this.authorizationService.getAuthorizationId().subscribe((x: number) => {
        this.authorizationRoleId = x;

        if(x == 3) { // if student, check if assignment is submittable
          this.submissionService.IsPastDue(this.assignmentId).subscribe(x => {
            this.pastDue = x;
          });
        }
      });
    });
  }

  /**
   * Checks if form input is valid
   * 
   * @param files file name
   */
  invalid(files): boolean {
    if (files.length == 0 || this.fileType == null || this.fileType == undefined)
      return true;

    return false;
  }

  /**
   * Submit the file to the assignment posting
   * 
   * @param files file name being submitted
   */
  public uploadFile = (files) => {
    if (files.length === 0) {
      return;
    }

    let fileToUpload = <File>files[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);

    this.submissionService.uploadAssignment(formData, this.assignmentId, this.courseId, this.fileType).subscribe(x => {
      this.navigateBack();
    });
  }

  //Navigate back to the previous page
  navigateBack() {
    this.router.navigate(['/courses', this.courseId]);
  }

}
