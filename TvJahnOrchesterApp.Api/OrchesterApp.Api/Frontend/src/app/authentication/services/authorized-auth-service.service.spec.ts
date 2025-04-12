import { TestBed } from '@angular/core/testing';

import { AuthorizedAuthServiceService } from './authorized-auth-service.service';

describe('AutherizedAuthServiceService', () => {
  let service: AuthorizedAuthServiceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthorizedAuthServiceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
