import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { AuthGuard } from './Guards/Auth/auth.guard';
import { AuthComponent } from './Components/auth/auth.component';
import { CoursesComponent } from './Components/courses/courses.component';
import { AssignmentsComponent } from './Components/assignments/assignments.component';
import { AddCourseComponent } from './Components/add-course/add-course.component';
import { AddAssignmentComponent } from './Components/add-assignment/add-assignment.component';
import { AssignmentSubmissionsComponent } from './Components/assignment-submissions/assignment-submissions.component';
import { SubmitAssignmentComponent } from './Components/submit-assignment/submit-assignment.component';
import { ErrorPageComponent } from './Components/error-page/error-page.component';
import { EditAssignmentComponent } from './Components/edit-assignment/edit-assignment.component';
import { EnrollmentComponent } from './Components/enrollment/enrollment.component';
import { ManageAccountComponent } from './Components/manage-account/manage-account.component';
import { ComparisonComponent } from './Components/comparison/comparison.component';
import { EditCourseComponent } from './Components/edit-course/edit-course.component';
import { UserManagementComponent } from './Components/user-management/user-management.component';
import { EditUserComponent } from './Components/edit-user/edit-user.component';
import { AddUserComponent } from './Components/add-user/add-user.component';
import { ProfessorGuard } from './Guards/Professor/professor.guard';
import { AdminGuard } from './Guards/Admin/admin.guard';
import { PlagiarismComponent } from './Components/plagiarism/plagiarism.component';
import { PlagiarismTestComponent } from './Components/plagiarism-test/plagiarism-test.component';

const routes: Routes = [
  // {
  //   path: '',
  //   component: AppComponent, canActivate: [AuthGuard],
  // },
  { path: '',
    redirectTo: '/courses',
    pathMatch: 'full'
  },
  {
    path: 'home',
    redirectTo: '/courses'
  },
  {
    path: 'auth',
    component: AuthComponent,
  },
  {
    path: 'error',
    component: ErrorPageComponent,
  },
  {
    path: 'courses',
    component: CoursesComponent, canActivate: [AuthGuard],
  },
  {
    path: 'courses/add',
    component: AddCourseComponent, canActivate: [AuthGuard, ProfessorGuard],
  },
  {
    path: 'courses/edit/:courseId',
    component: EditCourseComponent, canActivate: [AuthGuard, ProfessorGuard],
  },
  {
    path: 'courses/:courseId',
    component: AssignmentsComponent, canActivate: [AuthGuard],
  },
  {
    path: 'assignments/add/:courseId',
    component: AddAssignmentComponent, canActivate: [AuthGuard, ProfessorGuard],
  },
  {
    path: 'courses/edit/:courseId/:assignmentId',
    component: EditAssignmentComponent, canActivate: [AuthGuard, ProfessorGuard],
  },
  {
    path: 'courses/:courseId/submissions/:assignmentId',
    component: AssignmentSubmissionsComponent, canActivate: [AuthGuard, ProfessorGuard],
  },
  {
    path: 'courses/:courseId/submit/:assignmentId',
    component: SubmitAssignmentComponent, canActivate: [AuthGuard],
  },
  {
    path: 'enrollment',
    component: EnrollmentComponent, canActivate: [AuthGuard, ProfessorGuard],
  },
  {
    path: 'account',
    component: ManageAccountComponent, canActivate: [AuthGuard],
  },
  {
    path: 'compare/:guid',
    component: ComparisonComponent, canActivate: [AuthGuard, ProfessorGuard],
  },
  {
    path: 'users',
    component: UserManagementComponent, canActivate: [AuthGuard, AdminGuard], // change to AdminGuard
  },
  {
    path: 'users/edit/:userId',
    component: EditUserComponent, canActivate: [AuthGuard, AdminGuard]
  },
  {
    path: 'users/add',
    component: AddUserComponent, canActivate: [AuthGuard, AdminGuard]
  },
  {
    path: 'plagiarism',
    component: PlagiarismComponent, canActivate: [AuthGuard, ProfessorGuard]
  }, 
  {
    path: 'plagiarism/test',
    component: PlagiarismTestComponent, canActivate: [AuthGuard, ProfessorGuard]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
