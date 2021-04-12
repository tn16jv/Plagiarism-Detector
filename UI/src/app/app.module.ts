import { BrowserModule } from '@angular/platform-browser';
import { NgModule, Injector, CUSTOM_ELEMENTS_SCHEMA  } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpClientInterceptor } from './Interceptors/http-client.interceptor';
import { SideNavComponent } from './Components/side-nav/side-nav.component';
import { MaterialModule } from './Modules/material/material.module';
import { EnvSideNavComponent } from './Components/env-side-nav/env-side-nav.component';
import { NavEmitterService } from './Services/NavEmitter/nav-emitter.service';
import { FormsModule } from '@angular/forms';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatTabsModule } from '@angular/material/tabs';
import { MatExpansionModule } from '@angular/material/expansion';
import {MatRadioModule} from '@angular/material/radio';
import { MatTableModule } from '@angular/material/table';
import { MatSnackBarModule } from '@angular/material';
import { MatInputModule, MatTableDataSource, MatCheckboxModule, MatSelectModule, MatCardModule } from '@angular/material';
import { AuthComponent } from './Components/auth/auth.component';
import { HeaderComponent } from './Components/header/header.component';
import { MatIconModule } from '@angular/material';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatDialogModule } from '@angular/material/dialog';
import { ReactiveFormsModule } from '@angular/forms';
import { ClipboardModule } from 'ngx-clipboard';
import { MatTreeModule } from '@angular/material/tree';
import { CoursesComponent } from './Components/courses/courses.component';
import { AssignmentsComponent } from './Components/assignments/assignments.component';
import { AddCourseComponent } from './Components/add-course/add-course.component';
import { AddAssignmentComponent } from './Components/add-assignment/add-assignment.component';
import { CourseInfoComponent } from './Components/course-info/course-info.component';
import { AssignmentSubmissionsComponent } from './Components/assignment-submissions/assignment-submissions.component';
import { SubmitAssignmentComponent } from './Components/submit-assignment/submit-assignment.component';
import { ErrorPageComponent } from './Components/error-page/error-page.component';
import { EditAssignmentComponent } from './Components/edit-assignment/edit-assignment.component';
import { EnrollmentComponent } from './Components/enrollment/enrollment.component';
import { ConfirmDialogComponent } from './Components/confirm-dialog/confirm-dialog.component';
import { ManageAccountComponent } from './Components/manage-account/manage-account.component';
import { ComparisonComponent } from './Components/comparison/comparison.component';
import { EditCourseComponent } from './Components/edit-course/edit-course.component';
import { LoaderComponent } from './Components/loader/loader.component';
import { LoaderService } from './Services/Loader/loader.service';
import { UserManagementComponent } from './Components/user-management/user-management.component';
import { EditUserComponent } from './Components/edit-user/edit-user.component';
import { AddUserComponent } from './Components/add-user/add-user.component';
import { CommonModule } from '@angular/common';
import { PlagiarismComponent } from './Components/plagiarism/plagiarism.component';
import { PlagiarismTestComponent } from './Components/plagiarism-test/plagiarism-test.component';
import { DownloadAssignmentComponent } from './Components/download-assignment/download-assignment.component';
import { AssignmentInfoComponent } from './Components/assignment-info/assignment-info.component';

@NgModule({
  declarations: [
    AppComponent,
    SideNavComponent,
    EnvSideNavComponent,
    AuthComponent,
    HeaderComponent,
    CoursesComponent,
    AssignmentsComponent,
    AddCourseComponent,
    AddAssignmentComponent,
    CourseInfoComponent,
    AssignmentSubmissionsComponent,
    SubmitAssignmentComponent,
    ErrorPageComponent,
    EditAssignmentComponent,
    EnrollmentComponent,
    ConfirmDialogComponent,
    ManageAccountComponent,
    ComparisonComponent,
    EditCourseComponent,
    LoaderComponent,
    UserManagementComponent,
    EditUserComponent,
    AddUserComponent,
    PlagiarismComponent,
    PlagiarismTestComponent,
    DownloadAssignmentComponent,
    AssignmentInfoComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    MaterialModule,
    MatInputModule,
    MatIconModule,
    MatCheckboxModule,
    MatFormFieldModule,
    MatGridListModule,
    MatDialogModule,
    MatSelectModule,
    ReactiveFormsModule,
    MatCheckboxModule,
    MatExpansionModule,
    MatCardModule,
    MatTabsModule,
    MatRadioModule,
    MatTableModule,
    ClipboardModule,
    MatSnackBarModule,
    MatTreeModule,
    CommonModule,
  ],
  entryComponents: [
    ConfirmDialogComponent
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: HttpClientInterceptor,
      multi: true,
      deps: [ Injector, LoaderService ]
    },
    NavEmitterService,
  ],
  bootstrap: [AppComponent],
  schemas: [ CUSTOM_ELEMENTS_SCHEMA ]
})
export class AppModule {

}
