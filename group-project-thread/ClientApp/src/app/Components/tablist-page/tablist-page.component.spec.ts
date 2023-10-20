import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TablistPageComponent } from './tablist-page.component';

describe('ConnectionPageComponent', () => {
  let component: TablistPageComponent;
  let fixture: ComponentFixture<TablistPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TablistPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TablistPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
