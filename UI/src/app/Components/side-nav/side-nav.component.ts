import { Component, OnInit, Input } from '@angular/core';
import { NavEmitterService } from '../../Services/NavEmitter/nav-emitter.service';

@Component({
  selector: 'app-side-nav',
  templateUrl: './side-nav.component.html',
  styleUrls: ['./side-nav.component.scss']
})
export class SideNavComponent {

  expanded: boolean;  //if nav is expanded or not
  @Input() show: any; // = false;

  /**
   * Constructs the SideNav component which allows navigation across the site
   * 
   * @param navEmitterService service for toggling page size
   */
  constructor(private navEmitterService: NavEmitterService) {

    if (this.show === 'undefined') {
      this.show = false;
    }
    this.expanded = true;
   }

  //Toggles the main navigation tab 
  toggleMainNav() {
      this.show = !this.show;
      this.navEmitterService.togglePageSize(this.show);
   }
}
