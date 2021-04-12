import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  MatButtonModule,
  MatCardModule,
  MatListModule,
  MatSelectModule,
  MatFormFieldModule,
  MatTableModule,
  MatInputModule,
  MatPaginatorModule,
  MatSortModule,
  MatDialogModule,
  MatDatepickerModule,
  MatNativeDateModule,
  MatProgressSpinnerModule,

} from '@angular/material';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {MatTimeSelectModule, MatNativeTimeModule} from 'ngx-material-time-select';

@NgModule({
  imports: [
    CommonModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatCardModule,
    MatListModule,
    MatSelectModule,
    MatFormFieldModule,
    MatTableModule,
    MatInputModule,
    MatPaginatorModule,
    MatSortModule,
    MatDialogModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTimeSelectModule,
    MatNativeTimeModule,
    MatProgressSpinnerModule,
  ],
  exports: [
    BrowserAnimationsModule,
    MatButtonModule,
    MatCardModule,
    MatListModule,
    MatSelectModule,
    MatFormFieldModule,
    MatTableModule,
    MatInputModule,
    MatPaginatorModule,
    MatSortModule,
    MatDialogModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTimeSelectModule,
    MatNativeTimeModule,
    MatProgressSpinnerModule,
  ],
  declarations: []
})
export class MaterialModule { }
