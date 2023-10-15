import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FollowingPageUserComponent } from './following-page-user.component';

describe('FollowingPageUserComponent', () => {
  let component: FollowingPageUserComponent;
  let fixture: ComponentFixture<FollowingPageUserComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FollowingPageUserComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FollowingPageUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
