import {
  Component,
  OnInit,
  OnDestroy,
  ElementRef,
  AfterViewInit,
  AfterViewChecked,
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { MatSnackBar } from '@angular/material/snack-bar';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog } from '@angular/material/dialog';

import {
  Address,
  Country,
  County,
  Person,
  ExtendedAddress,
  AddressSummary,
  ExtendedAddressSummary,
  AddressFilter,
} from '../entities/entities';

import { normalizeAddressSummaries } from '../entities/utility';

import { LoadingWaiterComponent } from '../loading-waiter/loading-waiter.component';

import { ApiService } from '../services/api-service/api-service.service';

import { AddressSummaryComponent } from '../address-summary/address-summary.component';
import { AddressDetailsComponent } from '../address-details/address-details.component';

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
export class AddressBookComponent
  implements OnInit, AfterViewInit, AfterViewChecked, OnDestroy
{
  readonly controllerName = 'addresses';
  data: ExtendedAddressSummary[] | null = null;
  isLoading: boolean = false;
  isLoadingMore: boolean = false;
  hasLoadedAll: boolean = false;
  apiError: any | null;

  rootElem!: HTMLElement;
  mainElem!: HTMLElement;
  containerElem!: HTMLElement;

  constructor(
    private apiService: ApiService,
    private snackBar: MatSnackBar,
    private elRef: ElementRef,
    private editAddressDialog: MatDialog
  ) {
    this.rootElem = this.elRef.nativeElement;
    this.onScroll = this.onScroll.bind(this);
  }

  ngOnInit() {
    this.loadData();
  }

  ngAfterViewInit() {}

  ngAfterViewChecked() {
    this.containerElem = this.rootElem.querySelector(
      '.container'
    ) as HTMLElement;

    this.mainElem = this.rootElem.querySelector('.main-content') as HTMLElement;

    if (this.containerElem && this.mainElem) {
      this.containerElem.addEventListener('scroll', this.onScroll);
    }
  }

  ngOnDestroy() {
    if (this.containerElem) {
      this.containerElem.removeEventListener('scroll', this.onScroll);
    }
  }

  refreshButtonClick(e: MouseEvent) {
    this.loadData();
  }

  onScroll(e: Event) {
    // this.loadData();
    if (this.containerElem && this.mainElem) {
      const diff =
        this.mainElem.clientHeight -
        this.containerElem.scrollTop -
        window.innerHeight * 2;

      if (diff < 0) {
        this.loadMore();
      }
    }
  }

  async editClicked(address: AddressSummary) {
    this.openEditAddressDialog(address);
  }

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

  createClicked(e: MouseEvent) {
    this.openEditAddressDialog({} as AddressSummary);
  }

  openEditAddressDialog(address: AddressSummary) {
    this.editAddressDialog.open(AddressDetailsComponent, {
      data: address,
      width: 'calc(max(100vw - 10px, 1000px))',
      height: 'calc(100vh - 10px)',
    });
  }

  showError(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 10000, // Duration in milliseconds
      panelClass: ['error-snackbar'], // Optional custom class for styling
    });
  }

  private async loadData() {
    this.hasLoadedAll = false;
    this.isLoading = true;
    this.data = null;
    this.apiError = null;

    this.apiService
      .post<AddressSummary[]>(`${this.controllerName}/get-filtered`, {
        skipCount: 0,
        takeCount: 20,
      } as AddressFilter)
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

  private async loadMore() {
    if (!this.isLoadingMore && !this.hasLoadedAll) {
      this.isLoadingMore = true;

      this.apiService
        .post<AddressSummary[]>(`${this.controllerName}/get-filtered`, {
          skipCount: this.data?.length,
          takeCount: 20,
        } as AddressFilter)
        .subscribe({
          next: (response: AddressSummary[]) => {
            (this.data ??= []).splice(
              this.data.length - 1,
              0,
              ...normalizeAddressSummaries(response)
            );
            this.isLoadingMore = false;
            this.hasLoadedAll = response.length === 0;
          },
          error: (error) => {
            this.isLoadingMore = false;
            this.showError(`${error.status}: ${error.statusText}`);
          },
        });
    }
  }
}
