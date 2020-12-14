// @flow

export default function isPositive(value: ?number): boolean {
  if (value == null) return false;
  return parseInt(value) > 0;
}