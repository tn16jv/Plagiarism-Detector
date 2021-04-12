import { Component, OnInit } from '@angular/core';
import { UserModel } from 'src/app/Models/user-model';
import { UserService } from 'src/app/Services/UserService/user.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-add-user',
  templateUrl: './add-user.component.html',
  styleUrls: ['./add-user.component.scss']
})
export class AddUserComponent implements OnInit {

  user: UserModel;          //User being added
  studentNumRegex: RegExp;  //Regular Expression used for student number
  userId;                   //Id of user

  roles = [{ id: 0, value: "" },  //System roles
  { id: 1, value: "Professor" },
  { id: 2, value: "System Admin" },
  { id: 3, value: "Student" }];

  /**
   * Constructs the AddUser Component 
   * 
   * @param userService Service for adding User to DB
   * @param router router for navigation 
   * @param aRoute activated route
   */
  constructor(private userService: UserService, private router: Router, private aRoute: ActivatedRoute) {
    this.user = new UserModel();
    this.studentNumRegex = new RegExp('\\d{7}');
  }

  ngOnInit() {}

  /**
   * Adds the User to the DB then returns to the user page
   */
  addUser() {
    this.userService.addUser(this.user).subscribe(x => {
      this.navigateBack();
    });
  }

  /**
   * Checks if the user information is correct
   */
  notValid() {
    if (this.isEmpty(this.user.displayName) || this.isEmpty(this.user.email)) {
      return true;
    } else if (!this.isEmpty(this.user.studentNumber) && this.invalidStudentNum()) {
      return true;
    }
  }

  /**
   * Determines if a string is empty
   * 
   * @param value String being checked for emptiness
   */
  isEmpty(value: string) {
    if(value == undefined || value == null || value == "")
      return true;
  }

  /**
   * Determines if the student number is a 7 digit number
   */
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

  /**
   * Navigates to the previous page
   */
  navigateBack() {
    this.router.navigate(['/users']);
  }

}
