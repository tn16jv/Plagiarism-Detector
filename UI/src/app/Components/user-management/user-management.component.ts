import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/Services/UserService/user.service';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.scss']
})
export class UserManagementComponent implements OnInit {

  dataSource; //to store active users

  displayedColumns: string[] = [ //column headings in table
    'Name',
    'Email',
    'Role',
    'ViewDetails',
  ];

  /**
   * Constructs the UserManagement component for the admin to edit active users
   * 
   * @param userService service to get users
   */
  constructor(private userService: UserService) { }

  //Gets users on initialization
  ngOnInit() {
    this.getUsers();
  }

  //Get the active users of the system
  getUsers() {
    this.userService.getUsers().subscribe(x => {
      this.dataSource = x;
    });
  }

}
