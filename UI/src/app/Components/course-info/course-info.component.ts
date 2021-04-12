import { Component, OnInit, Input } from '@angular/core';
import { CourseService } from 'src/app/Services/Course/course.service';
import { CourseModel } from 'src/app/Models/course-model';

@Component({
  selector: 'app-course-info',
  templateUrl: './course-info.component.html',
  styleUrls: ['./course-info.component.scss']
})
export class CourseInfoComponent implements OnInit {


  @Input() courseId;    //id of course being displayed
  course: CourseModel;  //course being displayed

  /**
   * Constructs the CourseInfo component
   * 
   * @param courseService service for getting course from DB
   */
  constructor(private courseService: CourseService) {
    this.course = new CourseModel();
  }

  //Retrieve the course to be displayed based on the courseId
  ngOnInit() {
    this.courseService.getCourseInfo(this.courseId).subscribe(x => {
      this.course = x;
    });
  }

}
