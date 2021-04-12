import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EnvSideNavComponent } from './env-side-nav.component';

describe('EnvSideNavComponent', () => {
  let component: EnvSideNavComponent;
  let fixture: ComponentFixture<EnvSideNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EnvSideNavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EnvSideNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
