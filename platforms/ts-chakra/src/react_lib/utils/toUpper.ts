// Convert string to an upper-case string
export default function toUpper(value: string | null | undefined): string {
  if (value == null)
    return "";
  return value.toUpperCase();
}