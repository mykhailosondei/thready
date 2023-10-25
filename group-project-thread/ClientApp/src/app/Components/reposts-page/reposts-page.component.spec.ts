import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RepostsPageComponent } from './reposts-page.component';

describe('RepostsPageComponent', () => {
  let component: RepostsPageComponent;
  let fixture: ComponentFixture<RepostsPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RepostsPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RepostsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
