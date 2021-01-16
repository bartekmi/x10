// @flow

export default function isBlank(value: any): boolean {
  return value == null || String(value).trim() === "";
}