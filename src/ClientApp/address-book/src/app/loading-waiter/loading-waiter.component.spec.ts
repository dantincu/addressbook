import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoadingWaiterComponent } from './loading-waiter.component';

describe('LoadingWaiterComponent', () => {
  let component: LoadingWaiterComponent;
  let fixture: ComponentFixture<LoadingWaiterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LoadingWaiterComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoadingWaiterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
