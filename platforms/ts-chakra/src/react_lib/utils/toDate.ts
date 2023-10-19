export default function toDate(gqlDate?: string): Date | null {
  if (gqlDate == null) {
    return null;
  }
  return new Date(gqlDate);
}
