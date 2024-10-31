import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-free-text-or-lookup',
  standalone: true,
  imports: [],
  templateUrl: './free-text-or-lookup.component.html',
  styleUrl: './free-text-or-lookup.component.scss',
})
export class FreeTextOrLookupComponent {
  @Input()
  public title!: string;

  @Input()
  public placeholder?: string;

  @Input()
  public lookupApiUrl!: string;

  data: any[] | null = null;
  isLoading: boolean = false;
  apiError: any | null;
}
