import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MayBeInterestingPageComponent } from './may-be-interesting-page.component';

describe('MayBeInterestingPageComponent', () => {
  let component: MayBeInterestingPageComponent;
  let fixture: ComponentFixture<MayBeInterestingPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MayBeInterestingPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MayBeInterestingPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
