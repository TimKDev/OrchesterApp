import { TestBed } from '@angular/core/testing';

import { AnwesenheitService } from './anwesenheit.service';

describe('AnwesenheitService', () => {
  let service: AnwesenheitService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AnwesenheitService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
