import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { ConfirmModel } from 'src/app/Models/confirm-model';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.scss']
})

export class ConfirmDialogComponent {

  /**
   * Constructs the ConfirmDialog Component
   * 
   * @param dialogRef dialog reference
   * @param data text to be shown in confirm dialog
   */
  constructor(public dialogRef: MatDialogRef<ConfirmDialogComponent>, @Inject(MAT_DIALOG_DATA) public data: ConfirmModel) {}
  
  //Close dialog
  onNoClick(): void {
    this.dialogRef.close();
  }

}
