import { Component, OnInit } from '@angular/core';
import { CourseService } from 'src/app/Services/Course/course.service';
import { AuthorizationService } from 'src/app/Services/Authorization/authorization.service';

@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.scss']
})
export class CoursesComponent implements OnInit {

  dataSource;                       //courses being displayed
  authorizationRoleId;              //role of user

  displayedColumns: string[] = [    //column headers in table
    'Department',
    'CourseCode',
    'Duration',
    'Year',
    'ViewDetails',
  ];

  /**
   * Constructs the Courses Component to display all courses in a table
   * 
   * @param courseService service for getting courses from DB
   * @param authorizationService service for checking user role
   */
  constructor(private courseService: CourseService, private authorizationService: AuthorizationService) {}

  //Gets auth role and grabs courses from DB
  ngOnInit() {
    this.authorizationService.getAuthorizationId().subscribe((x:number) => {
      this.authorizationRoleId = x;
      this.getCourses();
    });
    
  }

  //Gets courses from DB that user is enrolled in
  getCourses() {
    if (this.authorizationRoleId == 1 || this.authorizationRoleId == 2) { // admin or prof
      this.courseService.getCourses().subscribe(x => {
        this.dataSource = x;
      });
    } 
    else if(this.authorizationRoleId == 3) { // student
      this.courseService.getStudentCourses().subscribe(x => {
        this.dataSource = x;
      });
    }
  }

}
