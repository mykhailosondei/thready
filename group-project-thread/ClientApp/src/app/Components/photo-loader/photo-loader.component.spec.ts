import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PhotoLoaderComponent } from './photo-loader.component';

describe('PhotoLoaderComponent', () => {
  let component: PhotoLoaderComponent;
  let fixture: ComponentFixture<PhotoLoaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PhotoLoaderComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PhotoLoaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
