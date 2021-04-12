import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-error-page',
  templateUrl: './error-page.component.html',
  styleUrls: ['./error-page.component.scss']
})
export class ErrorPageComponent implements OnInit {

  constructor() { }

  ngOnInit() {}

  //Returns the error message to be displayed upon activation
  getErrorMessage() {
    return localStorage.getItem("error_message");
  }
}
