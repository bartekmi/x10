using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  /// <summary>
  /// A "Hit" may have many matches from LexisNexis from multiple lists. Each such match is represented by this entity.
  /// </summary>
  public class MatchInfo : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? ReasonListed { get; set; }
    [GraphQLNonNullType]
    public string? Name { get; set; }
    [GraphQLNonNullType]
    public string? Address { get; set; }
    [GraphQLNonNullType]
    public string? Ids { get; set; }
    [GraphQLNonNullType]
    public string? MatchType { get; set; }
    public double? NameMatchScore { get; set; }
    public double? AddressMatchScore { get; set; }
    [GraphQLNonNullType]
    public string? Comments { get; set; }
    [GraphQLNonNullType]
    public string? RecordSource { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "MatchInfo: " + Dbid; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);
    }
  }
}

