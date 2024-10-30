import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

import { Address, Country, County, Person } from '../entities/entities';
import { dateToDisplayStr, moveUtcDateToLocalTime } from '../services/date';
import { LoadingWaiterComponent } from '../loading-waiter/loading-waiter.component';

import { ApiService } from '../services/api-service/api-service.service';

interface ExtendedAddress extends Address {
  createdAt: Date | string;
  lastModifiedAt?: Date | string | null | undefined;
}

@Component({
  selector: 'app-address-book',
  standalone: true,
  imports: [
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    LoadingWaiterComponent,
  ],
  templateUrl: './address-book.component.html',
  styleUrl: './address-book.component.scss',
})
export class AddressBookComponent implements OnInit {
  readonly controllerName = 'addresses';
  data: ExtendedAddress[] | null = null;
  isLoading: boolean = false;
  apiError: any | null;

  constructor(private apiService: ApiService) {
    this.transformData = this.transformData.bind(this);
  }

  ngOnInit() {
    this.loadData();
  }

  private async loadData() {
    this.isLoading = true;
    this.data = null;
    this.apiError = null;

    this.apiService.get<Address[]>(this.controllerName).subscribe({
      next: (response: Address[]) => {
        this.data = this.transformData(response);
        this.isLoading = false;
      },
      error: (error) => {
        this.apiError = JSON.stringify(error, null, '  ');
        this.isLoading = false;
      },
    });
  }

  private transformUtcDate(dateStr: string) {
    const dateNum = Date.parse(dateStr);
    const date = moveUtcDateToLocalTime(new Date(dateNum));
    const displayStr = dateToDisplayStr(date);
    return displayStr;
  }

  private transformData(inputData: Address[]) {
    const outputData = inputData.map(
      (data) =>
        ({
          ...data,
          createdAt: this.transformUtcDate(data.createdAtUtc),
          lastModifiedAt: data.lastModifiedAtUtc
            ? this.transformUtcDate(data.lastModifiedAtUtc)
            : null,
        } as ExtendedAddress)
    );

    return outputData;
  }
}
