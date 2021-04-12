import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  constructor() { }

  ngOnInit() { }

  //Gets the display name of the user logged into the site
  get userName() {
    return localStorage.getItem('name');
  }

}
