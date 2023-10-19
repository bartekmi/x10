using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.SmallSample.Repositories;

namespace x10.hotchoc.SmallSample.Entities {
  /// <summary>
  /// A physical or mailing address
  /// </summary>
  public class Address : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? UnitNumber { get; set; }
    [GraphQLNonNullType]
    public string? TheAddress { get; set; }
    [GraphQLNonNullType]
    public string? City { get; set; }
    [GraphQLNonNullType]
    public string? StateOrProvince { get; set; }
    [GraphQLNonNullType]
    public string? Zip { get; set; }

    // Associations
    [GraphQLNonNullType]
    public Country? Country { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? country = IdUtils.FromRelayId(Country?.Id);
      Country = country == null ? null : repository.GetCountry(country.Value);
    }
  }
}

