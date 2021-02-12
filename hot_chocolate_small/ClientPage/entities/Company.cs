using System;
using System.Collections.Generic;

using HotChocolate;

namespace x10.hotchoc.ClientPage.Entities {
  /// <summary>
  /// An entity we do business with - mostly an umbrella for [CompanyEntity]'s
  /// </summary>
  public class Company : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? Website { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "Company: " + Dbid; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public CompanyEntity? PrimaryEntity { get; set; }
    [GraphQLNonNullType]
    public List<CompanyEntity>? Entities { get; set; }
    [GraphQLNonNullType]
    public Contact? PrimaryContact { get; set; }
    [GraphQLNonNullType]
    public List<User>? Users { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      PrimaryEntity?.EnsureUniqueDbid();
      Entities?.ForEach(x => x.EnsureUniqueDbid());
      PrimaryContact?.EnsureUniqueDbid();
      Users?.ForEach(x => x.EnsureUniqueDbid());
    }
  }
}
