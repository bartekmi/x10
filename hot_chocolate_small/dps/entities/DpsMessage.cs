using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  /// <summary>
  /// An escalation message - logically identical to Core "Internal Message"
  /// </summary>
  public class DpsMessage : Base {
    // Regular Attributes
    public DateTime? Timestamp { get; set; }
    [GraphQLNonNullType]
    public string? Text { get; set; }
    public int? CoreShipmentId { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "DpsMessage: " + DbidHotChoc; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public User? User { get; set; }
    [GraphQLNonNullType]
    public List<DpsAttachment>? Attachments { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      Attachments?.ForEach(x => x.EnsureUniqueDbid());
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? user = IdUtils.FromFrontEndId(User?.Id);
      User = user == null ? null : repository.GetUser(user.Value);

      if (Attachments != null)
        foreach (DpsAttachment attachments in Attachments)
          attachments.SetNonOwnedAssociations(repository);
    }
  }
}

