import { Component, OnInit, Input } from '@angular/core';
import { AssignmentService } from 'src/app/Services/Assignment/assignment.service';
import { AssignmentInfoModel } from 'src/app/Models/assignment-info-model';

@Component({
  selector: 'app-assignment-info',
  templateUrl: './assignment-info.component.html',
  styleUrls: ['./assignment-info.component.scss']
})
export class AssignmentInfoComponent implements OnInit {

  @Input() assignmentId;            //ID of assignment being displayed
  assignment: AssignmentInfoModel;  //Assignment being displayed

  /**
   * Constructs the AssignmentInfo Component
   * 
   * @param assignmentService Service for getting Assignment to display
   */
  constructor(private assignmentService: AssignmentService) {
    this.assignment = new AssignmentInfoModel();
  }

  /**
   * Get assignment from DB on initiailiztion
   */
  ngOnInit() {
    this.assignmentService.getAssignmentInfo(this.assignmentId).subscribe(x => {
      this.assignment = x;
    });
  }

}
