import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecommendationsSideBarComponent } from './recommendations-side-bar.component';

describe('RecommendationsSideBarComponent', () => {
  let component: RecommendationsSideBarComponent;
  let fixture: ComponentFixture<RecommendationsSideBarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecommendationsSideBarComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecommendationsSideBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
