import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { AssignmentService } from 'src/app/Services/Assignment/assignment.service';
import { AuthorizationService } from 'src/app/Services/Authorization/authorization.service';
import { MatTableDataSource, MatDialog } from '@angular/material';
import { AssignmentModel } from 'src/app/Models/assignment-model';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-assignments',
  templateUrl: './assignments.component.html',
  styleUrls: ['./assignments.component.scss'],
})
export class AssignmentsComponent implements OnInit {

  courseId;             //Course id that assignments belong to
  authorizationRoleId;  //Role of user accessing page

  displayedColumns: string[] = [ //Columns in assignments table
    'AssignmentName',
    'DueDate',
    'LateDueDate',
    'SubmittedDate',
    'ViewDetails',
  ];
  dataSource = [{}];  //to hold assignment postings

  /**
   * Constructs the Assignments component to display assignment posting for a course
   * 
   * @param aRoute activated route
   * @param router router for navigation
   * @param assignmentService service for adding assignment
   * @param authorizationService service for getting role of user
   */
  constructor(private aRoute: ActivatedRoute, private router: Router, private assignmentService: AssignmentService,
    private authorizationService: AuthorizationService) { }

  /**
   * Activates upon initialization
   */
  ngOnInit() {
    this.aRoute.params.subscribe(params => {
      this.courseId = params['courseId'];
      this.authorizationService.getAuthorizationId().subscribe((x: number) => {
        this.authorizationRoleId = x;      
        this.getAssignments();
      });
    });
  }

  /**
   * Get all assignments for this course
   */
  getAssignments() {
    this.assignmentService.getAssignments(this.courseId, this.authorizationRoleId).subscribe(x => {      
      this.dataSource = x;
    });
  }

  /**
   * Checks if assignment can be submitted on time, late, or not at all.
   * 
   * @param assignment 
   */
  canSubmit(assignment: AssignmentModel) {
    let current = new Date();
    let dueDate = new Date(assignment.dueDate);
    let lateDueDate = new Date(assignment.lateDueDate);
    if (current > dueDate) {
      if (current > lateDueDate) {
        return -1; // past late due date, not submittable!
      }
      return 1; // past due date, submittable but will be marked as late
    }
    return 0; // gucci
  }

  /**
   * Checks if the assignment is unsubmitted
   * 
   * @param value assignment being checked 
   */
  unsubmitted(value: AssignmentModel) {
    let late = this.authorizationRoleId == 3
      && value.submittedDate == undefined
      && this.canSubmit(value) == -1;
    return late;
  }

  /**
   * Checks if the assignment is submitted late
   * 
   * @param value assignment being checked
   */
  isLate(value: AssignmentModel) {
    if (value.submittedDate == null)
      return false;
    let submittedDate = new Date(value.submittedDate);
    let dueDate = new Date(value.dueDate);
    let lateDueDate = new Date(value.lateDueDate);

    let late = this.authorizationRoleId == 3
      && submittedDate != undefined
      && submittedDate > dueDate
      && submittedDate <= lateDueDate;
    return late;
  }

  /**
   * Chekcs if the assignment is submitted on time
   * 
   * @param value assignment being checked
   */
  submittedOnTime(value: AssignmentModel) {
    if (value.submittedDate == null)
      return false;
    let submittedDate = new Date(value.submittedDate);
    let dueDate = new Date(value.dueDate);

    let late = this.authorizationRoleId == 3
      && submittedDate != undefined
      && submittedDate <= dueDate      
    return late;
  }

  //Gets columns to be displayed based on user role
  getColumns() {

    if (this.authorizationRoleId == 3) {
      return [
        'AssignmentName',
        'DueDate',
        'LateDueDate',
        'SubmittedDate',
        'ViewDetails'
      ];
    } else {
      return [
        'AssignmentName',
        'DueDate',
        'LateDueDate',
        'ViewDetails'
      ];
    }
  }

  //Navigates back to previous page
  navigateBack() {
    this.router.navigate(['/courses']);
  }
}
