using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.ClientPage.Repositories;

namespace x10.hotchoc.ClientPage.Entities {
  /// <summary>
  /// Our partner in the HK Security Program
  /// </summary>
  public class HkspPartnerUse : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? KcNumber { get; set; }
    public DateTime? ExpirationDate { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "HkspPartnerUse: " + DbidHotChoc; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public HkspPartner? Partner { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? partner = IdUtils.FromRelayId(Partner?.Id);
      Partner = partner == null ? null : repository.GetHkspPartner(partner.Value);
    }
  }
}

