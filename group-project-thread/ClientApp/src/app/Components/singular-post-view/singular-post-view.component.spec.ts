import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SingularPostViewComponent } from './singular-post-view.component';

describe('SingularPostViewComponent', () => {
  let component: SingularPostViewComponent;
  let fixture: ComponentFixture<SingularPostViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SingularPostViewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SingularPostViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
