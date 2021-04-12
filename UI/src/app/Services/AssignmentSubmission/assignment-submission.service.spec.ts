import { TestBed } from '@angular/core/testing';

import { AssignmentSubmissionService } from './assignment-submission.service';

describe('AssignmentSubmissionService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AssignmentSubmissionService = TestBed.get(AssignmentSubmissionService);
    expect(service).toBeTruthy();
  });
});
