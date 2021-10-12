// @flow

export default function x10toString(value: any): string {
  if (value == null || typeof value == 'number' && isNaN(value)) {
    return ""
  }
  return value.toString();
}