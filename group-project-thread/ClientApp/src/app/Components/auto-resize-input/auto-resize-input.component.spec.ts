import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AutoResizeInputComponent } from './auto-resize-input.component';

describe('AutoResizeInputComponent', () => {
  let component: AutoResizeInputComponent;
  let fixture: ComponentFixture<AutoResizeInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AutoResizeInputComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AutoResizeInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
