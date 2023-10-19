import isUuid from "./isUuid";

export default function isExistingObject(maybeUuid: string): boolean {
  return !isUuid(maybeUuid);
}