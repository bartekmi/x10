// Convert string to an lower-case string
export default function toLower(value: string | null | undefined): string {
  if (value == null)
    return "";
  return value.toLowerCase();
}