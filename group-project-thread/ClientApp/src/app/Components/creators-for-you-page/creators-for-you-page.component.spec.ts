import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreatorsForYouPageComponent } from './creators-for-you-page.component';

describe('CreatorsForYouPageComponent', () => {
  let component: CreatorsForYouPageComponent;
  let fixture: ComponentFixture<CreatorsForYouPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreatorsForYouPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreatorsForYouPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
