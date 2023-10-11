import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentCreationDialogComponent } from './comment-creation-dialog.component';

describe('CommentCreationDialogComponent', () => {
  let component: CommentCreationDialogComponent;
  let fixture: ComponentFixture<CommentCreationDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CommentCreationDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CommentCreationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
