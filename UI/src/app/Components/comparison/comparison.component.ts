import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from 'src/app/Services/Authorization/authorization.service';
import { ComparisonService } from 'src/app/Services/Comparison/comparison.service';
import { FileByLineModel, CopyFromModel, CopyModel, ComparisonResults } from 'src/app/Models/comparison-model';
import { map } from 'rxjs/operators';
import { UserService } from 'src/app/Services/UserService/user.service';
import { UserModel } from 'src/app/Models/user-model';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-comparison',
  templateUrl: './comparison.component.html',
  styleUrls: ['./comparison.component.scss']
})
export class ComparisonComponent implements OnInit {

  comparisonInfo: ComparisonResults = new ComparisonResults();  //results of comparison
  results: FileByLineModel[] = [new FileByLineModel()];         //list of FileByLineModel models
  selectedUser: FileByLineModel;                                //user currently selected
  authorizationRoleId: number;                                  //role of user
  leftSide: CopyFromModel;                                      //left page in report
  rightSide: CopyFromModel;                                     //right page in report
  users: UserModel[];                                           //list of users
  guid: string;                                                 //token to get comparison results

  /**
   * Constructs the Comparison component for showing plagairism detection results
   * 
   * @param comparisonService service for getting comparison results
   * @param userService service for getting users being compared
   * @param aRoute activated route
   */
  constructor(private comparisonService: ComparisonService, private userService: UserService, private aRoute: ActivatedRoute) { }

  ngOnInit() {
    this.aRoute.params.subscribe(params => {
      this.guid = params['guid'];
      this.getComparisonResults();
    });
  }

  //Gets the comparison results returned by the plagiarism engine
  getComparisonResults() {
    this.comparisonService.getCompare(this.guid).subscribe((x: ComparisonResults) => {
      this.comparisonInfo = x;
      this.results = x.userReports;
      console.log(x);
    });
    this.userService.getUsers().subscribe(x => {
      this.users = x;
    });
  }

  /**
   * Gets the user based on userId
   * 
   * @param userId id of user being returned
   */
  getUser(userId: number) {
    let userInfo = this.users.find(x => x.id == userId);
    return `${userInfo.accountName}-${userInfo.studentNumber}`;
  }

  //Get the user to be displayed on the right page of the report
  getRightHandUser() {
    let rightSideModel = this.results.find(x => x.userId == this.leftSide.copiedFromId);
    if (rightSideModel != null) {
      this.rightSide = rightSideModel.copyFrom.find(x => x.copiedFromId == this.selectedUser.userId);
      return true;
    }
    return false;
  }

  //Update the current selected user
  updateSelectedUser() {
    this.leftSide = null;
    this.rightSide = null;
  }

  //Update the current selected report
  updateSelectedReport() {
    this.rightSide = null;
  }

  //return key based on descending order
  descOrder = (a, b) => {
    if (a.key < b.key) return b.key;
  }

  /**
   * Scrolls to the line in a page based on id
   * 
   * @param id id of line being scrolled to
   */
  scroll(id: string) {
    var element = document.getElementById(id);
    if (element != null)
      element.scrollIntoView({ behavior: "smooth", block: "start" });
  }


}
