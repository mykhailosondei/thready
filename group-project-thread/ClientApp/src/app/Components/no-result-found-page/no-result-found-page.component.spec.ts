import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NoResultFoundPageComponent } from './no-result-found-page.component';

describe('NoResultFoundPageComponent', () => {
  let component: NoResultFoundPageComponent;
  let fixture: ComponentFixture<NoResultFoundPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NoResultFoundPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NoResultFoundPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
