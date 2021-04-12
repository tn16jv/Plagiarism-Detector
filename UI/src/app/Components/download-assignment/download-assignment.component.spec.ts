import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DownloadAssignmentComponent } from './download-assignment.component';

describe('DownloadAssignmentComponent', () => {
  let component: DownloadAssignmentComponent;
  let fixture: ComponentFixture<DownloadAssignmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DownloadAssignmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DownloadAssignmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
