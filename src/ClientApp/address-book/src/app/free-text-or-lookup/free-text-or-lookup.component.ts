import {
  Component,
  Input,
  Inject,
  ViewEncapsulation,
  AfterViewInit,
  AfterViewChecked,
  EventEmitter,
  Output,
} from '@angular/core';

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
import { ApiService } from '../services/api-service/api-service.service';
import { MatSelectModule, MatSelectChange } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';

import { CatalogueItem } from '../data/entities';

@Component({
  selector: 'app-free-text-or-lookup',
  standalone: true,
  imports: [
    CommonModule,
    MatInputModule,
    ReactiveFormsModule,
    FormsModule,
    MatSelectModule,
    MatOptionModule,
  ],
  templateUrl: './free-text-or-lookup.component.html',
  styleUrl: './free-text-or-lookup.component.scss',
})
export class FreeTextOrLookupComponent
  implements AfterViewInit, AfterViewChecked
{
  @Input()
  public caption!: string;

  @Input()
  public placeholder?: string;

  @Input()
  public lookupApiUrl!: string;

  @Input()
  public labelMemberName!: string;

  @Input()
  public idMemberName!: string;

  @Input()
  isRequired?: boolean;

  @Input()
  lookupIsActive?: boolean;

  @Input()
  showSelectionNothingOption?: boolean;

  @Input()
  selectionNothingOptionName?: string;

  @Output() currentChanged = new EventEmitter<CatalogueItem>();

  get isRequiredVal() {
    const requiredVal = this.isRequired ?? false;
    return requiredVal;
  }

  get placeholderVal() {
    const placeholderVal = this.placeholder ?? '';
    return placeholderVal;
  }

  get lookupIsActiveVal() {
    const lookupIsActive = this.lookupIsActive ?? true;
    return lookupIsActive;
  }

  currentLookupApiUrl: string = '';
  data: CatalogueItem[] = [];
  isLoading: boolean = false;
  apiError: any | null;

  currentId: string = '';
  current: CatalogueItem;

  userTextInput: string = '';

  constructor(private apiService: ApiService) {
    this.currentId = '';
    this.current = this.getSelectionNothingOption();
  }

  ngAfterViewInit() {}

  ngAfterViewChecked() {
    this.loadData();
  }

  userTextInputChanged(e: Event) {
    this.currentId = '';
    this.current = this.getSelectionNothingOption();

    this.currentChanged.emit({
      id: '',
      name: this.userTextInput,
    });
  }

  currentItemChanged(e: MatSelectChange) {
    this.current =
      this.data.find((item) => item.id === this.currentId) ??
      this.getSelectionNothingOption();

    this.userTextInput = this.current.name;
    this.currentChanged.emit(this.current);
  }

  getSelectionNothingOption() {
    const option: CatalogueItem = {
      id: '',
      name: this.selectionNothingOptionName ?? '',
    };

    return option;
  }

  private async loadData() {
    if (
      this.lookupIsActiveVal &&
      this.currentLookupApiUrl !== this.lookupApiUrl
    ) {
      this.currentLookupApiUrl = this.lookupApiUrl;
      this.currentId = '';
      this.current = this.getSelectionNothingOption();
      this.isLoading = true;
      this.data = [];
      this.apiError = null;

      this.apiService.get<any[]>(`${this.lookupApiUrl}`).subscribe({
        next: (response: any[]) => {
          if (this.lookupIsActiveVal) {
            this.data = response.map((row) => ({
              id: row[this.idMemberName],
              name: row[this.labelMemberName],
            }));

            this.data.splice(0, 0, {
              id: '',
              name: this.selectionNothingOptionName ?? '',
            });
          } else {
            this.data = [];
          }

          this.isLoading = false;
        },
        error: (error) => {
          this.apiError = error;
          this.isLoading = false;
        },
      });
    }
  }
}
