using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.ClientPage.Repositories;

namespace x10.hotchoc.ClientPage.Entities {
  /// <summary>
  /// VAT Number
  /// </summary>
  public class VatNumber : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? Number { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "VatNumber: " + DbidHotChoc; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public Country? CountryRegion { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? countryRegion = IdUtils.FromFrontEndId(CountryRegion?.Id);
      CountryRegion = countryRegion == null ? null : repository.GetCountry(countryRegion.Value);
    }
  }
}

