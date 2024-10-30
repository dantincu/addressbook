import { Component, Input } from '@angular/core';
import { MatCardModule } from '@angular/material/card';

import {
  Address,
  Country,
  County,
  Person,
  ExtendedAddress,
  AddressSummary,
  ExtendedAddressSummary,
} from '../entities/entities';

@Component({
  selector: 'app-address-summary',
  standalone: true,
  imports: [MatCardModule],
  templateUrl: './address-summary.component.html',
  styleUrl: './address-summary.component.scss',
})
export class AddressSummaryComponent {
  @Input() data!: ExtendedAddressSummary;
}
