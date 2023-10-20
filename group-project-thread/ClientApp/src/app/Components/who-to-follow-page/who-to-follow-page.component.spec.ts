import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WhoToFollowPageComponent } from './who-to-follow-page.component';

describe('WhoToFollowPageComponent', () => {
  let component: WhoToFollowPageComponent;
  let fixture: ComponentFixture<WhoToFollowPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ WhoToFollowPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(WhoToFollowPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
