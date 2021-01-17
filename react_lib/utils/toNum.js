// @flow

// This function is used to compare possibly null primitives which can be meaningfully
// converted to the "number" type. It takes advantage of the fact that any comparison
// with NaN returns false, which is exactly what we want - comparing against an 
// unknown quantity should be false - e.g. 1 is neither bigger nor smaller than unknown.

export default function toNum(value: ?number | ?Date): number {
  if (typeof value == "number") {
    return value;
  }

  if (value instanceof Date) {
    return value.getTime();
  }

  return NaN;
}