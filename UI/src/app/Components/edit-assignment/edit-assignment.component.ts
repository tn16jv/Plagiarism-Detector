import { Component, OnInit } from '@angular/core';
import { AssignmentModel } from 'src/app/Models/assignment-model';
import { ActivatedRoute, Router } from '@angular/router';
import { AssignmentService } from 'src/app/Services/Assignment/assignment.service';

@Component({
  selector: 'app-edit-assignment',
  templateUrl: './edit-assignment.component.html',
  styleUrls: ['./edit-assignment.component.scss']
})
export class EditAssignmentComponent implements OnInit {

  courseId: number;             //id of course that assign belongs to
  assignmentId: number;         //id of assignment being edited
  assignment: AssignmentModel;  //assignment being edited
  minDate: Date;                //minimum day displayed on date picker     

  /**
   * Consructs the EditAssignment Component, to edit an assignment
   * 
   * @param aRoute activated route
   * @param assignmentService service to get & update assignment
   * @param router router for navigation
   */
  constructor(private aRoute: ActivatedRoute, private assignmentService: AssignmentService, private router: Router) {
    this.assignment = new AssignmentModel();
   }

  //Activates upon initialization
  ngOnInit() {
    this.aRoute.params.subscribe(params => {      
      this.assignmentId = params['assignmentId'];
      this.courseId = params['courseId'];
      this.getAssignment();
      this.minDate = new Date();
      this.minDate.setDate(this.minDate.getDate());                
    });
  }

  //Gets assignment being edited from database
  getAssignment() {
    this.assignmentService.getAssignment(this.courseId, this.assignmentId).subscribe(x => {
      this.assignment = x;
      this.assignment.dueDate = new Date(this.assignment.dueDate);
      this.assignment.lateDueDate = new Date(this.assignment.lateDueDate);
    });    
  }

  // Updates the assignment in the database
  updateAssignment() {    
    this.assignmentService.updateAssignment(this.assignment).subscribe(x => {      
      this.navigateBack();
    });
  }

  // checks that all fields have values & late due date is after due date
  notValid() {    
    if((this.assignment.assignmentName == undefined || this.assignment.assignmentName == "") || 
       (this.assignment.dueDate == undefined || this.assignment.lateDueDate == undefined) ||       
       this.pastDueDate()) {
      
      return true;
    } 
  }

  // checks if due date is before due date
  pastDueDate() {
    if(this.assignment.dueDate != undefined && this.assignment.lateDueDate != undefined){         
      if(this.assignment.dueDate > this.assignment.lateDueDate){
        return true;
      }  
    }             
  }     

  // Navigates back to the previous page
  navigateBack() {
    this.router.navigate(['/courses', this.assignment.courseId]);
  }

}
