import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlagiarismTestComponent } from './plagiarism-test.component';

describe('PlagiarismTestComponent', () => {
  let component: PlagiarismTestComponent;
  let fixture: ComponentFixture<PlagiarismTestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlagiarismTestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlagiarismTestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
