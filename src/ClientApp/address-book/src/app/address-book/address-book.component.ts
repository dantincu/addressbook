import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import {
  Address,
  Country,
  County,
  Person,
  ExtendedAddress,
  AddressSummary,
  ExtendedAddressSummary,
} from '../entities/entities';

import { normalizeAddressSummaries } from '../entities/utility';

import { dateToDisplayStr, moveUtcDateToLocalTime } from '../services/date';
import { LoadingWaiterComponent } from '../loading-waiter/loading-waiter.component';

import { ApiService } from '../services/api-service/api-service.service';

import { AddressSummaryComponent } from '../address-summary/address-summary.component';

@Component({
  selector: 'app-address-book',
  standalone: true,
  imports: [
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    LoadingWaiterComponent,
    AddressSummaryComponent,
  ],
  templateUrl: './address-book.component.html',
  styleUrl: './address-book.component.scss',
})
export class AddressBookComponent implements OnInit {
  readonly controllerName = 'addresses';
  data: ExtendedAddressSummary[] | null = null;
  isLoading: boolean = false;
  apiError: any | null;

  constructor(private apiService: ApiService, private snackBar: MatSnackBar) {}

  ngOnInit() {
    this.loadData();
  }

  refreshButtonClick(e: MouseEvent) {
    this.loadData();
  }

  private async loadData() {
    this.isLoading = true;
    this.data = null;
    this.apiError = null;

    this.apiService
      .post<AddressSummary[]>(`${this.controllerName}/get-filtered`, {})
      .subscribe({
        next: (response: AddressSummary[]) => {
          this.data = normalizeAddressSummaries(response);
          this.isLoading = false;
        },
        error: (error) => {
          this.apiError = error;
          this.isLoading = false;

          this.showError(`${error.status}: ${error.statusText}`);
        },
      });
  }

  async editClicked(address: AddressSummary) {}

  async deleteClicked(address: AddressSummary) {
    address.hasPendingApiCall = true;
    this.apiService.delete(`${this.controllerName}/${address.id}`).subscribe({
      next: () => {
        const idx = this.data!.findIndex((addr) => addr.id === address.id);

        if (idx >= 0) {
          this.data!.splice(idx, 1);
        }

        address.hasPendingApiCall = false;
      },
      error: (error) => {
        this.showError(`${error.status}: ${error.statusText}`);
        address.hasPendingApiCall = false;
      },
    });
  }

  showError(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 10000, // Duration in milliseconds
      panelClass: ['error-snackbar'], // Optional custom class for styling
    });
  }
}
