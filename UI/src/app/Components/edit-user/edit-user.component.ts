import { Component, OnInit } from '@angular/core';
import { UserModel } from 'src/app/Models/user-model';
import { UserService } from 'src/app/Services/UserService/user.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-edit-user',
  templateUrl: './edit-user.component.html',
  styleUrls: ['./edit-user.component.scss']
})
export class EditUserComponent implements OnInit {

  user: UserModel;            //User being edited 
  studentNumRegex: RegExp;    //regular expression for student num validation 
  userId;                     //id of user being edited

  roles = [{ id: 0, value: "" },    //User roles
  { id: 1, value: "Professor" },
  { id: 2, value: "System Admin" },
  { id: 3, value: "Student" }];

  /**
   * Constructs the EditUser component for editing a user
   * 
   * @param userService service for getting and updating a user
   * @param router router for navigation
   * @param aRoute activated route
   */
  constructor(private userService: UserService, private router: Router, private aRoute: ActivatedRoute) {
    this.user = new UserModel();
    this.studentNumRegex = new RegExp('\\d{7}');
  }

  //Activated upon initialization 
  ngOnInit() {
    this.aRoute.params.subscribe(params => {
      this.userId = params['userId'];
      this.getUser();
    });
  }

  //Get the user being edited 
  getUser() {
    this.userService.getUserById(this.userId).subscribe(x => {
      this.user = x;
    });
  }

  //Update the user with new info
  updateUser() {
    this.userService.adminUpdateUser(this.user).subscribe(x => {
      this.navigateBack();
    });
  }

  //Checks if form input is valid
  notValid() {
    if (this.isEmpty(this.user.displayName) || this.isEmpty(this.user.email)) {
      return true;
    } else if (!this.isEmpty(this.user.studentNumber) && this.invalidStudentNum()) {
      return true;
    }
  }

  //Checks if form input is valid
  isEmpty(value: string) {
    if(value == undefined || value == null || value == "")
      return true;
  }

  //Checks if the student number is formatted correctly
  invalidStudentNum() {
    if (this.user.studentNumber != undefined && this.user.studentNumber != "") {
      if (this.user.studentNumber.length > 7) {
        return true;
      }
      else {
        return !this.studentNumRegex.test(this.user.studentNumber);
      }
    }
    return false;
  }

  //Navigates back to the previous page
  navigateBack() {
    this.router.navigate(['/users']);
  }
}
