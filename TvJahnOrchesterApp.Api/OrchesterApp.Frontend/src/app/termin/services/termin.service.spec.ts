import { TestBed } from '@angular/core/testing';

import { TerminService } from './termin.service';

describe('TerminService', () => {
  let service: TerminService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TerminService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
