using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.SmallSample.Repositories;

namespace x10.hotchoc.SmallSample.Entities {
  /// <summary>
  /// Somewhat contrived move event from one apartment to another
  /// </summary>
  public class Move : Base {
    // Regular Attributes
    public DateTime? Date { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "Move: " + DbidHotChoc; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public Building? From { get; set; }
    [GraphQLNonNullType]
    public Building? To { get; set; }
    [GraphQLNonNullType]
    public Tenant? Tenant { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? from = IdUtils.FromRelayId(From?.Id);
      From = from == null ? null : repository.GetBuilding(from.Value);

      int? to = IdUtils.FromRelayId(To?.Id);
      To = to == null ? null : repository.GetBuilding(to.Value);

      int? tenant = IdUtils.FromRelayId(Tenant?.Id);
      Tenant = tenant == null ? null : repository.GetTenant(tenant.Value);
    }
  }
}

