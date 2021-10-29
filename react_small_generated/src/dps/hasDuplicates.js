// @flow

// Return true if there are any MatchInfos which have a match on 'address'
export default function hasDuplicates(items: ?$ReadOnlyArray<any>, field: string): boolean {
  if (items == null) return true;
  const array = items.map(x => x[field]);
  return (new Set(array)).size !== array.length;
}