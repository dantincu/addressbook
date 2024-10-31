import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FreeTextOrLookupComponent } from './free-text-or-lookup.component';

describe('FreeTextOrLookupComponent', () => {
  let component: FreeTextOrLookupComponent;
  let fixture: ComponentFixture<FreeTextOrLookupComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FreeTextOrLookupComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FreeTextOrLookupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
