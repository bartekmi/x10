// @flow

import isUuid from "./isUuid";

export default function isNewObject(maybeUuid: string): boolean {
  return isUuid(maybeUuid);
}