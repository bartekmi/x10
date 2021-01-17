// @flow

export default function toDate(gqlDate: ?string): ?Date {
  if (gqlDate == null) {
    return null;
  }
  return new Date(gqlDate);
}
