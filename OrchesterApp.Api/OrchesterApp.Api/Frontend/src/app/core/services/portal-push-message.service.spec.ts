import { TestBed } from '@angular/core/testing';

import { PortalPushMessageService } from './portal-push-message.service';

describe('PortalPushMessageService', () => {
  let service: PortalPushMessageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PortalPushMessageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
