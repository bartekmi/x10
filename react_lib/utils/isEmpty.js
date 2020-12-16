// @flow

export default function isEmpty(value: ?string | ?number): boolean {
  return value == null || String(value).trim() === "";
}