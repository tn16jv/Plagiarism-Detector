import { Component, OnInit } from '@angular/core';
import { CourseModel } from 'src/app/Models/course-model';
import { CourseService } from 'src/app/Services/Course/course.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-edit-course',
  templateUrl: './edit-course.component.html',
  styleUrls: ['./edit-course.component.scss']
})
export class EditCourseComponent implements OnInit {

  courseId: number;         //id of course being edited
  course: CourseModel;      //course being edited
  date: Date;               //used for course year dropdown
  courseCodeRegex: RegExp;    //regular expression for validating course code

  durations = [ "", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D10", "D11" ]; //Course durations
  departments = ["", "COSC", "APCO", "IASC", "MATH"];                                     //Faculty deparments
  years = [];                                                                             //Course years  

  /**
   * Constructs the EditCourse Component for editing a course
   * 
   * @param aRoute activated route
   * @param courseService service for getting and updating a course
   * @param router router for navigation
   */
  constructor(private aRoute: ActivatedRoute, private courseService: CourseService, private router: Router) {
    this.course = new CourseModel();
    this.courseCodeRegex = new RegExp('\\d\\D\\d\\d');
    this.date = new Date();    
    for (var i = 5; i >= -1; i--) {
      this.years.push(this.date.getFullYear()-i);      
    }       
   }

  //Gets course being edited
  ngOnInit() {
    this.aRoute.params.subscribe(params => {      
      this.courseId = params['courseId'];
      this.getCourse();               
    });
  }

  //Gets course being edited based on courseId
  getCourse() {    
    this.courseService.getCourse(this.courseId).subscribe(x => {
      this.course = x;
    });       
  }

  //Update course with new information
  updateCourse() {
    this.courseService.updateCourse(this.course).subscribe(x => {
      this.navigateBack();
    });
  }

  //Checks that form input is valid 
  notValid() {
    if((this.course.courseCode == undefined || this.course.departmentName == undefined || this.course.duration == undefined || this.course.year == undefined)
    || (this.course.courseCode == "" || this.course.departmentName == "" || this.course.duration == "" || this.course.year == "")
    || this.invalidCourseCode()) {
      return true;
    }
  }

  //Checks that the course code is formatted correctly
  invalidCourseCode(){
    if(this.course.courseCode != undefined){
      return !this.courseCodeRegex.test(this.course.courseCode);
    }
    return false;
  }

  //Navigates to the previous page
  navigateBack() {
    this.router.navigate(['/courses']);
  }

}
