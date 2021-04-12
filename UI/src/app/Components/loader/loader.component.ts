import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LoaderService } from 'src/app/Services/Loader/loader.service';
import { LoaderState } from 'src/app/Models/loader-state';

@Component({
  selector: 'app-loader',
  templateUrl: './loader.component.html',
  styleUrls: ['./loader.component.scss']
})
export class LoaderComponent implements OnInit {

  show = false;                         //to show the loading icon or not
  private subscription: Subscription;   //gets the loader state
  
  constructor(private loaderService: LoaderService) { }
  
  //Shows the loading icon on initialization
  ngOnInit() {
    this.subscription = this.loaderService.loaderState
    .subscribe((state: LoaderState) => {
      this.show = state.show;
    });
  }

  //Destroys the loading icon once completed
  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

}
