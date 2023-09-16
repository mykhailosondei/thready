import { TestBed } from '@angular/core/testing';

import { HttpInternalService } from './http-internal.service';

describe('HttpInternalService', () => {
  let service: HttpInternalService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HttpInternalService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
