import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PostCreationDialogComponent } from './post-creation-dialog.component';

describe('PostCreationDialogComponent', () => {
  let component: PostCreationDialogComponent;
  let fixture: ComponentFixture<PostCreationDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PostCreationDialogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PostCreationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
