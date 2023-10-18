import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TopBlurryBarComponent } from './top-blurry-bar.component';

describe('TopBlurryBarComponent', () => {
  let component: TopBlurryBarComponent;
  let fixture: ComponentFixture<TopBlurryBarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TopBlurryBarComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TopBlurryBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
