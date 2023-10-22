import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeletableImageComponent } from './deletable-image.component';

describe('DeletableImageComponent', () => {
  let component: DeletableImageComponent;
  let fixture: ComponentFixture<DeletableImageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DeletableImageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeletableImageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
