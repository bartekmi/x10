// @flow

export function getYear(dateAsString: ?string): number {
  if (dateAsString == null) {
    return NaN;
  }
  const date = new Date(dateAsString);
  return date.getFullYear();
}

export function getDate(timestampAsString: ?string): ?string {
  if (timestampAsString == null) {
    return null;
  }

  const splitIndex = timestampAsString.indexOf("Z");
  if (splitIndex == -1) {
    return timestampAsString;
  }

  return timestampAsString.substring(0, splitIndex);
}