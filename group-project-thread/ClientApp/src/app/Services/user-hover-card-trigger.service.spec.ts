import { TestBed } from '@angular/core/testing';

import { UserHoverCardTriggerService } from './user-hover-card-trigger.service';

describe('UserHoverCardTriggerService', () => {
  let service: UserHoverCardTriggerService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(UserHoverCardTriggerService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
