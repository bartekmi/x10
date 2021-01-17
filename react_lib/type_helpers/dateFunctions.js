// @flow

export function getYear(dateAsString: ?string): number {
  if (dateAsString == null) {
    return NaN;
  }
  const date = new Date(dateAsString);
  return date.getFullYear();
}