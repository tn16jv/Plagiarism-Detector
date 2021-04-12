import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlagiarismComponent } from './plagiarism.component';

describe('PlagiarismComponent', () => {
  let component: PlagiarismComponent;
  let fixture: ComponentFixture<PlagiarismComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlagiarismComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlagiarismComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
