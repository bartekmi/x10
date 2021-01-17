// @flow

export function getYear(date: ?Date): number {
  if (date == null) {
    return NaN;
  }
  return date.getFullYear();
}
