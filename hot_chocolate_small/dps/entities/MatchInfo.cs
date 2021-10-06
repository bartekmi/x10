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
    public string? SourceList { get; set; }
    [GraphQLNonNullType]
    public string? Ids { get; set; }
    [GraphQLNonNullType]
    public string? Name { get; set; }
    [GraphQLNonNullType]
    public string? Address { get; set; }
    [GraphQLNonNullType]
    public string? Type { get; set; }
    [GraphQLNonNullType]
    public string? Title { get; set; }
    [GraphQLNonNullType]
    public string? Source { get; set; }
    [GraphQLNonNullType]
    public string? Programs { get; set; }
    public double? Score { get; set; }
    [GraphQLNonNullType]
    public string? Note { get; set; }

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

