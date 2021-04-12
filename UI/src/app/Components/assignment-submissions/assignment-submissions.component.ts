import { Component, OnInit } from '@angular/core';
import { AssignmentSubmissionService } from 'src/app/Services/AssignmentSubmission/assignment-submission.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ComparisonService } from 'src/app/Services/Comparison/comparison.service';

@Component({
  selector: 'app-assignment-submissions',
  templateUrl: './assignment-submissions.component.html',
  styleUrls: ['./assignment-submissions.component.scss']
})
export class AssignmentSubmissionsComponent implements OnInit {

  assignmentId;       //id of assignment
  courseId;           //id of course that assign belongs to
  dataSource;         //holds the submissions to this assignment
  hasDownload = true; // required for download-assignment component (destroy if no download found)

  displayedColumns: string[] = [  //Columns to be displayed in table
    'UserName',
    'StudentNumber',
    'FileType',
    'SubmittedDate',
    'ViewDetails',
  ];

  /**
   * Constructs the AssignmentSubmission Component
   * 
   * @param aRoute activated route
   * @param submissionService service to get all assignment submissions
   * @param comparisonService service to check submissions for plagairism
   * @param router router for navigation
   */
  constructor(private aRoute: ActivatedRoute, private submissionService: AssignmentSubmissionService, 
    private comparisonService: ComparisonService, private router: Router) { }

  /**
   * Activates upon initialization, gets all submissions
   */
  ngOnInit() {
    this.aRoute.params.subscribe(params => {
      this.courseId = params['courseId'];
      this.assignmentId = params['assignmentId'];
      this.getAssignmentSubmissions();
    });
  }

  /**
   * Gets all submissions to this assignment
   */
  getAssignmentSubmissions() {
    this.submissionService.getAssignmentSubmissions(this.assignmentId).subscribe(x => {
      this.dataSource = x;
    });
  }

  /**
   * Sends submissions to the comparison engine to check for plagiarism
   */
  compareAssignments() {
    this.comparisonService.compareAssignments(this.assignmentId).subscribe(x => {
      this.router.navigate(['/compare/', x.guid]);
    });
  }

}
