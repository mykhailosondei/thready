import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FollowersPageUserComponent } from './followers-page-user.component';

describe('FollowersPageUserComponent', () => {
  let component: FollowersPageUserComponent;
  let fixture: ComponentFixture<FollowersPageUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FollowersPageUserComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FollowersPageUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
