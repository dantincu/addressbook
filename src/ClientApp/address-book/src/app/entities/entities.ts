export interface EntityBase<TPk> {
  id: TPk;
}

export interface TimeStampEntity {
  createdAtUtc: string;
  lastModifiedAtUtc?: string | null | undefined;
}

export interface ExtendedTimeStampEntity extends TimeStampEntity {
  createdAt: string;
  lastModifiedAt?: string | null | undefined;
  lastModifiedOrCreatedStr: string;
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

export interface Address extends EntityBase<number>, TimeStampEntity {
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
}

export interface AddressSummary extends EntityBase<number>, TimeStampEntity {
  firstName?: string;
  middleName?: string;
  lastName?: string;
  fullName: string;
  countryName: string;
  countyName: string;
  cityName: string;
  hasPendingApiCall?: boolean | null | undefined;
}

export interface ExtendedAddress extends Address, ExtendedTimeStampEntity {}

export interface ExtendedAddressSummary
  extends AddressSummary,
    ExtendedTimeStampEntity {}

export interface AddressFilter {
  skipCount: number;
  takeCount: number;
}
