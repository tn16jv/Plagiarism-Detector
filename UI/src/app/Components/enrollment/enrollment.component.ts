import { Component, OnInit } from '@angular/core';
import { CourseService } from 'src/app/Services/Course/course.service';
import { AuthorizationService } from 'src/app/Services/Authorization/authorization.service';
import { CourseModel } from 'src/app/Models/course-model';
import { EnrollmentService } from 'src/app/Services/Enrollment/enrollment.service';
import { EnrollStudentModel } from 'src/app/Models/enroll-student-model';
import { ConfirmDialogComponent } from '../confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material';

@Component({
  selector: 'app-enrollment',
  templateUrl: './enrollment.component.html',
  styleUrls: ['./enrollment.component.scss']
})
export class EnrollmentComponent implements OnInit {
  
  authorizationRoleId: number;                              //Role of user accessing page
  dataSource: any;                                          //users enrolled in course 
  courseList: CourseModel[];                                //list of courses belonging to prof
  selectedCourseId: number;                                 //id of course currently selected
  enroll: EnrollStudentModel = new EnrollStudentModel();    //model to hold student emails & course id

  displayedColumns: string[] = [  //column headers in table
    'StudentEmail',
    'StudentNumber',
    'Details',
  ];

  /**
   * Constructs the Enrollment component for enrolling user to a course
   * 
   * @param enrollmentService service for getting enrolled users & enrolling new ones
   * @param courseService service for getting courses
   * @param authorizationService service for checking user roles
   * @param dialog confirm dialog for removing students
   */
  constructor(private enrollmentService: EnrollmentService, private courseService: CourseService, private authorizationService: AuthorizationService, private dialog: MatDialog) {}

  //Gets user role and associated courses on initialization
  ngOnInit() {
    this.authorizationService.getAuthorizationId().subscribe((x:number) => {
      this.authorizationRoleId = x;
      this.getCourses();
    });
  }

  //Get courses that user is running
  getCourses() {
    if (this.authorizationRoleId == 1 || this.authorizationRoleId == 2) { // admin or prof
      this.courseService.getCourses().subscribe(x => { // get courses for prof
        this.courseList = x;
      });
    } 
  }

  //Get students enrolled in selected course
  getEnrolledStudents() {
    this.enrollmentService.getEnrolledStudents(this.selectedCourseId).subscribe(x => { // get courses for prof
      this.dataSource = x;
    });
  }

  //Enroll new students to selected course 
  enrollStudents() {
    this.enroll.courseId = this.selectedCourseId;

    this.enrollmentService.enrollStudents(this.enroll).subscribe(x => {
      this.enroll.studentEmails = "";
      this.getEnrolledStudents(); // if successful, update table to display changes
    })
  }

  /**
   * Remove selected student from selected course
   * 
   * @param studentId id of student being removed from course
   */
  removeStudentFromCourse(studentId: number): void {
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      width: '250px',
      data: {type: "Delete", message: "Are you sure you want to remove the student from this course?"}
    });

    dialogRef.afterClosed().subscribe(result => {      
      if(result){
        this.enrollmentService.removeStudentFromCourse(studentId, this.selectedCourseId).subscribe(x => {
          this.getEnrolledStudents();
        })
      }
    });
  }

  //Checks if form input is valid
  invalid() {
    if (this.enroll.studentEmails == undefined || this.enroll.studentEmails == null || this.enroll.studentEmails == '')
      return true;
    return false;
  }

}
