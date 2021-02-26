using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.ClientPage.Repositories;

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

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      PrimaryEntity?.SetNonOwnedAssociations(repository);

      if (Entities != null)
        foreach (CompanyEntity entities in Entities)
          entities.SetNonOwnedAssociations(repository);

      PrimaryContact?.SetNonOwnedAssociations(repository);

      if (Users != null)
        foreach (User users in Users)
          users.SetNonOwnedAssociations(repository);
    }
  }
}

