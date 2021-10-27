using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  /// <summary>
  /// Part of Settings. Determines who gets auto-assigned to Hit based on time.
  /// </summary>
  public class SettingsAutoAssignment : Base {
    // Regular Attributes
    public string? From { get; set; }
    public string? To { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "SettingsAutoAssignment: " + DbidHotChoc; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public User? User { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? user = IdUtils.FromRelayId(User?.Id);
      User = user == null ? null : repository.GetUser(user.Value);
    }
  }
}

