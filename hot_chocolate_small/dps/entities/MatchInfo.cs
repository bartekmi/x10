using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  // Enums
  public enum MatchTypeEnum {
    Business,
    Individual,
    Vessel,
  }


  /// <summary>
  /// A "Hit" may have many matches from LexisNexis from multiple lists. Each such match is represented by this entity.
  /// </summary>
  public class MatchInfo : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public int? Number { get; set; }
    [GraphQLNonNullType]
    public string? ReasonListed { get; set; }
    [GraphQLNonNullType]
    public string? Name { get; set; }
    [GraphQLNonNullType]
    public string? Address { get; set; }
    [GraphQLNonNullType]
    public MatchTypeEnum? MatchType { get; set; }
    [GraphQLNonNullType]
    public int? Score { get; set; }
    [GraphQLNonNullType]
    public int? NameMatchScore { get; set; }
    [GraphQLNonNullType]
    public int? AddressMatchScore { get; set; }
    [GraphQLNonNullType]
    public string? Comments { get; set; }
    [GraphQLNonNullType]
    public string? RecordSource { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "MatchInfo: " + DbidHotChoc; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public List<MatchInfoSource>? Sources { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      Sources?.ForEach(x => x.EnsureUniqueDbid());
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      if (Sources != null)
        foreach (MatchInfoSource sources in Sources)
          sources.SetNonOwnedAssociations(repository);
    }
  }
}

