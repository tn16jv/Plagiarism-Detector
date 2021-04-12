import { Component, OnInit } from '@angular/core';
import { AssignmentService } from 'src/app/Services/Assignment/assignment.service';
import { AssignmentModel } from 'src/app/Models/assignment-model';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-add-assignment',
  templateUrl: './add-assignment.component.html',
  styleUrls: ['./add-assignment.component.scss']
})
export class AddAssignmentComponent implements OnInit {

  assignment: AssignmentModel;  //assignment being added
  minDate: Date;                //minimum day displayed on date picker 

  /**
   * Constructs the AddAssignment Component
   * 
   * @param aRoute activated route
   * @param assignmentService Service for adding Assignment to DB
   * @param router router for navigation
   */
  constructor(private aRoute: ActivatedRoute, private assignmentService: AssignmentService, private router: Router) {
    this.assignment = new AssignmentModel();
   }

  /**
   * Activate upon initialztion 
   */
  ngOnInit() {
    this.aRoute.params.subscribe(params => {      
      this.assignment.courseId = params['courseId'];
      this.minDate = new Date();
      this.minDate.setDate(this.minDate.getDate());          
    });
  }

  /**
   *  Adds the new assignment to the database
   */
  addAssignment() {    
    this.assignmentService.addAssignment(this.assignment).subscribe(x => {      
      this.navigateBack();
    });
  }

  /**
   * Checks that all fields have values & late due date is after due date 
   */
  notValid() {    
    if((this.assignment.assignmentName == undefined || this.assignment.assignmentName == "") || 
       (this.assignment.dueDate == undefined || this.assignment.lateDueDate == undefined) ||       
       this.pastDueDate()) {
      
      return true;
    } 
  }

  /**
   * Checks if due date is before due date  
   **/
  pastDueDate() {
    if(this.assignment.dueDate != undefined && this.assignment.lateDueDate != undefined){         
      if(this.assignment.dueDate > this.assignment.lateDueDate){
        return true;
      }  
    }             
  }     

  /**
   * Navigates back to the previous page
   */
  navigateBack() {
    this.router.navigate(['/courses', this.assignment.courseId]);
  }

}
