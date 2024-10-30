export interface EntityBase<TPk> {
  id: TPk;
}

export interface Country extends EntityBase<number> {
  name: string;
  code: string;
}

export interface County extends EntityBase<number> {
  name: string;
  countryId: number;
  country: Country;
}

export interface Person extends EntityBase<number> {
  firstName: string;
  middleName: string;
  lastName: string;
}

export interface Address extends EntityBase<number> {
  countryName?: string | null | undefined;
  countyName?: string | null | undefined;
  cityName: string;
  streetType?: string | null | undefined;
  streetName: string;
  streetNumber: string;
  postalCode: string;
  blockNumber?: string | null | undefined;
  stairNumber?: string | null | undefined;
  floorNumber?: string | null | undefined;
  apartmentNumber?: string | null | undefined;
  personId: number;
  countryId?: number | null | undefined;
  countrId?: number | null | undefined;
  country?: Country | null | undefined;
  county?: County | null | undefined;
  person: Person;
  createdAtUtc: string;
  lastModifiedAtUtc?: string | null | undefined;
}
