import {
  Component,
  Inject,
  ViewEncapsulation,
  Output,
  EventEmitter,
  AfterViewInit,
  ChangeDetectorRef,
} from '@angular/core';

import { Observable } from 'rxjs';

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
import { MatSnackBar } from '@angular/material/snack-bar';

import {
  Address,
  AddressCore,
  Person,
  ExtendedAddressSummary,
  ExtendedAddress,
} from '../data/entities';

import { ApiService } from '../services/api-service/api-service.service';

import { LoadingWaiterComponent } from '../loading-waiter/loading-waiter.component';

import { FreeTextOrLookupComponent } from '../free-text-or-lookup/free-text-or-lookup.component';

import { CatalogueItem } from '../data/entities';
import { isRequiredCatalogueItemValid } from '../data/utility';

import {
  getValidationErrors,
  PropertyValidation,
  ValidationError,
  getValidationErrorsSummaryText,
} from '../data/form-validations';

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
    LoadingWaiterComponent,
  ],
  templateUrl: './address-details.component.html',
  styleUrl: './address-details.component.scss',
  encapsulation: ViewEncapsulation.None,
})
export class AddressDetailsComponent implements AfterViewInit {
  readonly controllerName = 'addresses';

  @Output()
  public saved = new EventEmitter<Address>();

  @Output()
  public deleted = new EventEmitter<number>();

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

  isCallingApi: boolean = false;
  apiError: any | null = null;
  propertyValidations: PropertyValidation[] | null = null;
  address: Address | null = null;

  get countyLookupIsEnabled() {
    const countyLookupIsEnabled = !!this.country.id;
    return countyLookupIsEnabled;
  }

  constructor(
    private cdr: ChangeDetectorRef,
    public dialogRef: MatDialogRef<AddressDetailsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ExtendedAddressSummary,
    private snackBar: MatSnackBar,
    private apiService: ApiService
  ) {
    this.country = this.getEmptyCatalogueItem();
    this.county = this.getEmptyCatalogueItem();
  }

  get isEdit() {
    const isEdit = !!this.data.id;
    return isEdit;
  }

  ngAfterViewInit() {
    if (this.isEdit) {
      this.loadData();
    }
  }

  onSave(): void {
    this.propertyValidations = null;

    if (
      this.form.invalid ||
      !(
        isRequiredCatalogueItemValid(this.country) &&
        isRequiredCatalogueItemValid(this.county)
      )
    ) {
      this.propertyValidations = getValidationErrors(this.form);

      const validationSummaryText = getValidationErrorsSummaryText(
        this.propertyValidations
      );

      this.showError(validationSummaryText);
    } else {
      const formData = this.getFormData();
      let observable: Observable<any>;

      if (this.isEdit) {
        observable = this.apiService.put(
          `${this.controllerName}/${this.data.id}`,
          formData
        );
      } else {
        observable = this.apiService.post(
          `${this.controllerName}/${this.data.id}`,
          formData
        );
      }

      observable.subscribe({
        next: () => {
          this.saved.emit(formData);
          this.isCallingApi = false;
          this.dialogRef.close();
        },
        error: (error) => {
          this.apiError = error;
          this.showError(`${error.status}: ${error.statusText}`);
          this.isCallingApi = false;
        },
      });
    }
  }

  onClose(): void {
    this.dialogRef.close();
  }

  onDelete(): void {
    this.isCallingApi = true;

    this.apiService.delete(`${this.controllerName}/${this.data.id}`).subscribe({
      next: () => {
        this.deleted.emit(this.data.id);
        this.isCallingApi = false;
        this.dialogRef.close();
      },
      error: (error) => {
        this.showError(`${error.status}: ${error.statusText}`);
        this.isCallingApi = false;
      },
    });
  }

  loadData() {
    setTimeout(() => {
      this.isCallingApi = true;
    }, 0);

    this.apiService
      .get<Address>(`${this.controllerName}/${this.data.id}`)
      .subscribe({
        next: (result) => {
          this.isCallingApi = false;
          this.setFormData(result);
        },
        error: (error) => {
          this.showError(`${error.status}: ${error.statusText}`);
          this.isCallingApi = false;
        },
      });
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

  getFormData() {
    const data = {
      id: this.data.id,
      countryId: this.country.id ? parseInt(this.country.id) : null,
      countryName: this.country.id ? null : this.country.name,
      countyId: this.county.id ? parseInt(this.county.id) : null,
      countyName: this.county.id ? null : this.county.name,
      cityName: this.form.value.cityName!,
      streetType: this.form.value.streetType!,
      streetName: this.form.value.streetName!,
      streetNumber: this.form.value.streetNumber!,
      postalCode: this.form.value.postalCode!,
      blockNumber: this.form.value.blockNumber!,
      stairNumber: this.form.value.stairNumber!,
      floorNumber: this.form.value.floorNumber!,
      apartmentNumber: this.form.value.apartmentNumber!,
      person: {
        firstName: this.form.value.firstName!,
        middleName: this.form.value.middleName,
        lastName: this.form.value.lastName!,
      } as Person,
    } as Address;

    return data;
  }

  setFormData(data: Address) {
    this.address = data;

    this.country = {
      id: data.countryId?.toString() ?? '',
      name: data.countryName!,
    };

    this.county = {
      id: data.countyId?.toString() ?? '',
      name: data.countyName!,
    };

    data = {
      ...data,
    };

    const patchData = {
      ...data,
      firstName: data.person.firstName,
      middleName: data.person.middleName,
      lastName: data.person.lastName,
    };

    // @ts-ignore
    delete patchData.person;

    console.log('patchData', patchData);

    this.form.patchValue({
      // @ts-ignore
      patchData,
    });
  }

  showError(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 10000, // Duration in milliseconds
      panelClass: ['error-snackbar'], // Optional custom class for styling
    });
  }
}
