import { TestBed, inject } from '@angular/core/testing';

import { NavEmitterService } from './nav-emitter.service';

describe('NavEmitterService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [NavEmitterService]
    });
  });

  it('should be created', inject([NavEmitterService], (service: NavEmitterService) => {
    expect(service).toBeTruthy();
  }));
});
