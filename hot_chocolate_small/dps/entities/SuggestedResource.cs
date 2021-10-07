using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  // Enums
  public enum SuggestedResourceTypeEnum {
    Google,
    LinkedIn,
  }

  public enum HelpfulStateEnum {
    Unspecified,
    Helpful,
    Unhelpful,
  }


  /// <summary>
  /// Canned search results from sources like Google and LinkedIn which may be helpful in making a clear/don't-clear decision
  /// </summary>
  public class SuggestedResource : Base {
    // Regular Attributes
    public SuggestedResourceTypeEnum? Type { get; set; }
    [GraphQLNonNullType]
    public string? Title { get; set; }
    [GraphQLNonNullType]
    public string? Text { get; set; }
    public HelpfulStateEnum? Helpful { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "SuggestedResource: " + Dbid; }
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

