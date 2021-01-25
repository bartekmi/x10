// @flow

export default function isUuid(maybeUuid: string): bool {
  return maybeUuid.split("-").length - 1 >= 4;
}
