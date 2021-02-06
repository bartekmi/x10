using System;
using System.Collections.Generic;

using HotChocolate;

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
      get { return "CompanyEntityCountryService: " + Dbid; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public Country? Country { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }
  }
}

