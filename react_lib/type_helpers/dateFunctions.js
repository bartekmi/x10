// @flow

export function gqlToDate(gqlDate: ?string): ?Date {
  if (gqlDate == null) {
    return null;
  }
  return new Date(gqlDate);
}

export function getYear(date: ?Date): number {
  if (date == null) {
    return NaN;
  }
  return date.getFullYear();
}

export function dateGreaterThan(a: ?Date, b: ?Date): boolean {
  if (a == null || b == null) {
    return false;
  }

  return a > b;
}

export function dateLessThan(a: ?Date, b: ?Date): boolean {
  if (a == null || b == null) {
    return false;
  }

  return a < b;
}
