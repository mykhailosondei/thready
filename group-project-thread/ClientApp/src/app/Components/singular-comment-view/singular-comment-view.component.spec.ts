import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SingularCommentViewComponent } from './singular-comment-view.component';

describe('SingularCommentViewComponent', () => {
  let component: SingularCommentViewComponent;
  let fixture: ComponentFixture<SingularCommentViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SingularCommentViewComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SingularCommentViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
