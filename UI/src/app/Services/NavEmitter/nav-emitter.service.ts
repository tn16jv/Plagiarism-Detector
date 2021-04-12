import { Injectable, EventEmitter } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NavEmitterService {

  secondaryNav: EventEmitter<string> = new EventEmitter();
  public toggleMainSideNav: EventEmitter<boolean> = new EventEmitter();
  constructor() {
    this.secondaryNav.emit('hidden');
  }

  //Toggles page size
  togglePageSize(toggle: boolean) {
    this.toggleMainSideNav.emit(toggle);
  }

  public set values(values: Map<string, string>) {
    values.forEach((key: string, value: string) => {
      localStorage[key] = value;
    });
  }

  public value(key) {
    return localStorage[key];
  }

  public add(key, value) {
    localStorage[key] = value;
  }
}
