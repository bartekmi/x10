// This function is used to convert enums to their nominal comparable values
// In particular, enum values may come from the back-end with non-standard capitalization
// so we force the value to lower case

export default function toEnum(value?: string): string | null {
  if (value == null) {
    return null;
  }

  return value.toLocaleLowerCase();
}