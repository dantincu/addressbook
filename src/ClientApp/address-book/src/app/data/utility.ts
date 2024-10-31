import { dateToDisplayStr, moveUtcDateToLocalTime } from '../services/date';

import {
  CatalogueItem,
  Address,
  AddressSummary,
  ExtendedAddress,
  ExtendedAddressSummary,
  TimeStampEntity,
  ExtendedTimeStampEntity,
} from './entities';

export const transformUtcDate = (dateStr: string) => {
  const dateNum = Date.parse(dateStr);
  const date = moveUtcDateToLocalTime(new Date(dateNum));
  const displayStr = dateToDisplayStr(date);
  return displayStr;
};

export const getLastModifiedOrCreatedStr = (
  createdAt: string,
  lastModifiedAt: string | null | undefined
) =>
  lastModifiedAt
    ? `Last modified at ${lastModifiedAt}`
    : `Created at ${createdAt}`;

export const normalizeEntity = <
  TEntity extends TimeStampEntity,
  TExtendedEntity extends ExtendedTimeStampEntity
>(
  inputData: TEntity
) => {
  const retData = {
    ...inputData,
    createdAt: transformUtcDate(inputData.createdAtUtc),
    lastModifiedAt: inputData.lastModifiedAtUtc
      ? transformUtcDate(inputData.lastModifiedAtUtc)
      : null,
  } as TimeStampEntity as TExtendedEntity;

  retData.lastModifiedOrCreatedStr = getLastModifiedOrCreatedStr(
    retData.createdAt,
    retData.lastModifiedAt
  );

  return retData;
};

export const normalizeAddressSummary = (data: AddressSummary) =>
  normalizeEntity<AddressSummary, ExtendedAddressSummary>(data);

export const normalizeAddress = (data: Address) =>
  normalizeEntity<Address, ExtendedAddress>(data);

export const normalizeAddressSummaries = (inputData: AddressSummary[]) =>
  inputData.map(normalizeAddressSummary);

export const normalizeAddresses = (inputData: Address[]) =>
  inputData.map(normalizeAddress);

export const isRequiredCatalogueItemValid = (item: CatalogueItem) => {
  const isValid = !!(item.id || item.name);
  return isValid;
};
