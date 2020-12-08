// @flow

export default function isEmpty(value: ?string): boolean {
  return value == null || value.trim() === "";
}