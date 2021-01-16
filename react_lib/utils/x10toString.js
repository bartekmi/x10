// @flow

export default function x10toString(value: any): string {
  if (value == null || isNaN(value)) {
    return ""
  }
  return value.toString();
}