import { TestBed } from '@angular/core/testing';

import { UnauthorizedHttpClientService } from './unauthorized-http-client.service';

describe('UnauthorizedHttpClientService', () => {
  let service: UnauthorizedHttpClientService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UnauthorizedHttpClientService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
