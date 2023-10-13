import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UserHoverCardComponent } from './user-hover-card.component';

describe('UserHoverCardComponent', () => {
  let component: UserHoverCardComponent;
  let fixture: ComponentFixture<UserHoverCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ UserHoverCardComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UserHoverCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
