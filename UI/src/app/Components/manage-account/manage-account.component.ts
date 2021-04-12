import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/Services/UserService/user.service';
import { Router } from '@angular/router';
import { UserModel } from 'src/app/Models/user-model';

@Component({
  selector: 'app-manage-account',
  templateUrl: './manage-account.component.html',
  styleUrls: ['./manage-account.component.scss']
})
export class ManageAccountComponent implements OnInit {

  user: UserModel;          //user being managed
  studentNumRegex: RegExp;  //regular expression to check student number

  /**
   * Constructs the ManageAccount component for the logged in user to modify their account info
   * 
   * @param userService service for getting & updating user
   * @param router router for navigation
   */
  constructor(private userService: UserService, private router: Router) { 
    this.user = new UserModel();
    this.studentNumRegex = new RegExp('\\d{7}');
  }

  //Gets the user model on initialization 
  ngOnInit() {
    this.userService.getUser().subscribe(x => {
      this.user = x;
    });
  }

  //Update the user with new information
  updateAccount() {
    this.userService.updateUser(this.user).subscribe(x => {
      this.navigateBack();
    });
  }

  //Checks if form input is valid
  notValid() {
    if(this.user.studentNumber == undefined || this.user.displayName == undefined || 
       this.user.studentNumber == "" || this.user.displayName == "" || this.invalidStudentNum()){
      return true;
    }                 
  }

  //Checks if the student number is valid
  invalidStudentNum(){
    if(this.user.studentNumber != undefined){
      if(this.user.studentNumber.length > 7){
        return true;
      }
      else{
        return !this.studentNumRegex.test(this.user.studentNumber);
      }
    }    
    return false;
  }

  //Navigates to the previous page
  navigateBack() {
    this.router.navigate(['/home']);
  }

}
