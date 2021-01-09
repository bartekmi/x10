// @flow

// Return true if object is undefined, null or a string with only whitespace in it
export default function isBlank(obj: any) {
  if (obj == null)
    return true;
  return obj.toString().trim() === "";
}