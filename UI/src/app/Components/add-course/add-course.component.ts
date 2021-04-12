import { Component, OnInit } from '@angular/core';
import { CourseService } from 'src/app/Services/Course/course.service';
import { CourseModel } from 'src/app/Models/course-model';
import { Location } from '@angular/common';
import { yearsPerPage } from '@angular/material/datepicker/typings/multi-year-view';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-course',
  templateUrl: './add-course.component.html',
  styleUrls: ['./add-course.component.scss']
})
export class AddCourseComponent implements OnInit {

  course: CourseModel;      //Course being added
  date: Date;               //Date used for course year dropdown 
  courseCodeRegex: RegExp;  //Regular Expression for validating course code

  durations = [ "", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9", "D10", "D11" ]; //Course durations
  departments = ["", "COSC", "APCO", "IASC", "MATH"];                                     //Faculty deparments 
  years = [];                                                                             //Course Years


  /**
   * Constructs the AddCourse Component 
   * 
   * @param courseService Service used for adding courses to db
   * @param router router for navigation
   */
  constructor(private courseService: CourseService, private router: Router) {
    this.course = new CourseModel();
    this.courseCodeRegex = new RegExp('\\d\\D\\d\\d');
    this.date = new Date();    
    for (var i = 5; i >= -1; i--) {
      this.years.push(this.date.getFullYear()-i);      
    }
   }

  ngOnInit() {}

  /**
   * Adds the course to the db
   */
  addCourse() {
    this.courseService.addCourse(this.course).subscribe(x => {
      this.navigateBack();
    });
  }

  /**
   * checks that all fields have values
   */ 
  notValid() {
    if((this.course.courseCode == undefined || this.course.departmentName == undefined || this.course.duration == undefined || this.course.year == undefined)
    || (this.course.courseCode == "" || this.course.departmentName == "" || this.course.duration == "" || this.course.year == "")
    || this.invalidCourseCode()) {
      return true;
    }
  }

  /**
   * Checks if course code is formatted correct
   */
  invalidCourseCode(){
    if(this.course.courseCode != undefined){
      return !this.courseCodeRegex.test(this.course.courseCode);
    }
    return false;
  }

  /**
   * Navigates back to the previous page
   */
  navigateBack() {
    this.router.navigate(['/courses']);
  }

}
