using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.ClientPage.Repositories;

namespace x10.hotchoc.ClientPage.Entities {
  /// <summary>
  /// A physical or mailing address
  /// </summary>
  public class Address : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? TheAddress { get; set; }
    [GraphQLNonNullType]
    public string? TheAddress2 { get; set; }
    [GraphQLNonNullType]
    public string? City { get; set; }
    [GraphQLNonNullType]
    public string? PostalCode { get; set; }
    [GraphQLNonNullType]
    public bool Verified { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "Address: " + Dbid; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public Country? Country { get; set; }
    [GraphQLNonNullType]
    public StateOrProvince? StateOrProvince { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? country = IdUtils.FromRelayId(Country?.Id);
      Country = country == null ? null : repository.GetCountry(country.Value);

      int? stateOrProvince = IdUtils.FromRelayId(StateOrProvince?.Id);
      StateOrProvince = stateOrProvince == null ? null : repository.GetStateOrProvince(stateOrProvince.Value);
    }
  }
}

