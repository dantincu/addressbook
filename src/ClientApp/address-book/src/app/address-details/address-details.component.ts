import { Component, Inject, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatDialogRef } from '@angular/material/dialog';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormGroup, FormControl } from '@angular/forms';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { ExtendedAddressSummary } from '../entities/entities';

import { ApiService } from '../services/api-service/api-service.service';

import {
  FreeTextOrLookupComponent,
  CatalogueItem,
} from '../free-text-or-lookup/free-text-or-lookup.component';

@Component({
  selector: 'app-address-details',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    FormsModule,
    FreeTextOrLookupComponent,
  ],
  templateUrl: './address-details.component.html',
  styleUrl: './address-details.component.scss',
  encapsulation: ViewEncapsulation.None,
})
export class AddressDetailsComponent {
  form = new FormGroup({
    firstName: new FormControl(''),
    middleName: new FormControl<string | null>(null),
    lastName: new FormControl(''),
    cityName: new FormControl(''),
    streetType: new FormControl(''),
    streetName: new FormControl(''),
    streetNumber: new FormControl(''),
    postalCode: new FormControl<string | null>(null),
    blockNumber: new FormControl<string | null>(null),
    stairNumber: new FormControl<string | null>(null),
    floorNumber: new FormControl<string | null>(null),
    apartmentNumber: new FormControl<string | null>(null),
  });

  country: CatalogueItem;
  county: CatalogueItem;

  get countyLookupIsEnabled() {
    const countyLookupIsEnabled = !!this.country.id;
    return countyLookupIsEnabled;
  }

  constructor(
    public dialogRef: MatDialogRef<AddressDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ExtendedAddressSummary,
    private apiService: ApiService
  ) {
    this.country = this.getEmptyCatalogueItem();
    this.county = this.getEmptyCatalogueItem();
  }

  get isEdit() {
    const isEdit = !!this.data.id;
    return isEdit;
  }

  onSave(): void {
    this.dialogRef.close();
  }

  onClose(): void {
    this.dialogRef.close();
  }

  onDelete(): void {
    this.dialogRef.close();
  }

  countryChanged(e: CatalogueItem) {
    this.country = e;
    this.county = this.getEmptyCatalogueItem();
  }

  countyChanged(e: CatalogueItem) {
    this.county = e;
  }

  getEmptyCatalogueItem() {
    const item: CatalogueItem = {
      id: '',
      name: '',
    };

    return item;
  }
}
