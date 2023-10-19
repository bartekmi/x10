export default function isUuid(maybeUuid: string): boolean {
  return maybeUuid.split("-").length - 1 >= 4;
}
