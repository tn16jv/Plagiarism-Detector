import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AssignmentSubmissionService } from 'src/app/Services/AssignmentSubmission/assignment-submission.service';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-download-assignment',
  templateUrl: './download-assignment.component.html',
  styleUrls: ['./download-assignment.component.scss']
})
export class DownloadAssignmentComponent implements OnInit {


  @Input() assignmentId: number;          //id of assign being downloaded
  @Input() studentId: number;             //id of student who owns assign
  @Input() delete: boolean = false;       //if assignment is deleted
  @Output() close = new EventEmitter();   //event emitter

  fileName: string;                       //name of file being downloaded

  /**
   * Constructs the DownloadAssignment Component
   * 
   * @param submissionService service used to get filename and download file
   */
  constructor(private submissionService: AssignmentSubmissionService) { }

  //Gets file name on init
  ngOnInit() {
    this.getFileName();
  }

  //Gets submitted file based on assignment id and student id
  getFileName() {
    this.submissionService.getDownloadFileName(this.assignmentId, this.studentId).subscribe(data => {
      if (data == null)
        this.close.emit(null);
      else 
        this.fileName = data.fileName;
    });
  }

  //Downloads file based on assignment id and student id
  downloadFile() {
    this.submissionService.downloadAssignment(this.assignmentId, this.studentId).subscribe(data => {
      const blob = new Blob([data as BlobPart], { type: 'application/zip' });
      saveAs(blob, this.fileName);
    });
  }

  //Deletes file from DB based on assignment id and student id
  deleteFile() {
    this.submissionService.deleteFile(this.assignmentId, this.studentId).subscribe(data => {
      this.getFileName();
    });
  }

}
