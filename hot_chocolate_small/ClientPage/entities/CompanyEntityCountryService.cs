using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.ClientPage.Repositories;

namespace x10.hotchoc.ClientPage.Entities {
  /// <summary>
  /// A country and an indication wherther either of import/export services are required
  /// </summary>
  public class CompanyEntityCountryService : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public bool ImportCustoms { get; set; }
    [GraphQLNonNullType]
    public bool ExportCustoms { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "CompanyEntityCountryService: " + DbidHotChoc; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public Country? Country { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? country = IdUtils.FromFrontEndId(Country?.Id);
      Country = country == null ? null : repository.GetCountry(country.Value);
    }
  }
}

