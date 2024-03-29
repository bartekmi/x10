// @flow

type MatchInfo = {
  +addressMatchScore: ?number,
};

// Return true if there are any MatchInfos which have a match on 'address'
export default function hasAddressMatches(matches: ?$ReadOnlyArray<MatchInfo>): boolean {
  if (matches == null) return false;
  return matches.some(x => x.addressMatchScore && x.addressMatchScore > 85);
}