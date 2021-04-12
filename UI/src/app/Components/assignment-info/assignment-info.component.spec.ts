import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssignmentInfoComponent } from './assignment-info.component';

describe('AssignmentInfoComponent', () => {
  let component: AssignmentInfoComponent;
  let fixture: ComponentFixture<AssignmentInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssignmentInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssignmentInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
