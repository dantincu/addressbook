import {
  Component,
  Input,
  OnInit,
  OnDestroy,
  ElementRef,
  AfterViewInit,
  EventEmitter,
  Output,
} from '@angular/core';

import { MatCardModule } from '@angular/material/card';
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
} from '../data/entities';

@Component({
  selector: 'app-address-summary',
  standalone: true,
  imports: [MatCardModule, MatButtonModule, MatIconModule],
  templateUrl: './address-summary.component.html',
  styleUrl: './address-summary.component.scss',
})
export class AddressSummaryComponent
  implements OnInit, AfterViewInit, OnDestroy
{
  rootElem!: HTMLElement;
  deleteBtn!: HTMLElement;

  @Input() data!: ExtendedAddressSummary;

  @Output() editClicked = new EventEmitter<ExtendedAddressSummary>();
  @Output() deleteClicked = new EventEmitter<ExtendedAddressSummary>();

  constructor(private elRef: ElementRef) {
    this.elementClicked = this.elementClicked.bind(this);
  }

  ngOnInit() {}

  ngAfterViewInit() {
    this.rootElem = this.elRef.nativeElement;
    this.deleteBtn = this.rootElem.querySelector('.delete-btn') as HTMLElement;

    this.rootElem.addEventListener('click', this.elementClicked, {
      capture: true,
    });
  }

  ngOnDestroy() {
    this.rootElem.removeEventListener('click', this.elementClicked, {
      capture: true,
    });
  }

  elementClicked(e: MouseEvent) {
    var composedPathResult = e.composedPath();

    if (composedPathResult.indexOf(this.deleteBtn) >= 0) {
      this.deleteClicked.emit(this.data);
    } else {
      this.editClicked.emit(this.data);
    }
  }
}
