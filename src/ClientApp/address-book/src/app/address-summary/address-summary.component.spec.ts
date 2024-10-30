import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddressSummaryComponent } from './address-summary.component';

describe('AddressSummaryComponent', () => {
  let component: AddressSummaryComponent;
  let fixture: ComponentFixture<AddressSummaryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddressSummaryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddressSummaryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
