using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  /// <summary>
  /// Records an edit to a Hit after the initial resolution
  /// </summary>
  public class HitEdit : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? EditedFields { get; set; }
    public DateTime? Timestamp { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "HitEdit: " + DbidHotChoc; }
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

      int? user = IdUtils.FromFrontEndId(User?.Id);
      User = user == null ? null : repository.GetUser(user.Value);
    }
  }
}

