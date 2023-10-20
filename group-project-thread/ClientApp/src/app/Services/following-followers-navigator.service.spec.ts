import { TestBed } from '@angular/core/testing';

import { FollowingFollowersNavigatorService } from './following-followers-navigator.service';

describe('FollowingFollowersNavigatorService', () => {
  let service: FollowingFollowersNavigatorService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FollowingFollowersNavigatorService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
