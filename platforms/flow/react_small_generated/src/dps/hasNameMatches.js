// @flow

type MatchInfo = {
  +nameMatchScore: ?number,
};

// Return true if there are any MatchInfos which have a match on 'name'
export default function hasNameMatches(matches: ?$ReadOnlyArray<MatchInfo>): boolean {
  if (matches == null) return false;
  return matches.some(x => x.nameMatchScore && x.nameMatchScore > 85);
}