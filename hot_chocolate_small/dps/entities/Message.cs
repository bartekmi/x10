using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  /// <summary>
  /// An escalation message - logically identical to Core "Internal Message"
  /// </summary>
  public class Message : Base {
    // Regular Attributes
    public DateTime? Timestamp { get; set; }
    [GraphQLNonNullType]
    public string? Text { get; set; }
    public int? CoreShipmentId { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "Message: " + Dbid; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public User? User { get; set; }
    [GraphQLNonNullType]
    public List<Attachment>? Attachments { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      Attachments?.ForEach(x => x.EnsureUniqueDbid());
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? user = IdUtils.FromRelayId(User?.Id);
      User = user == null ? null : repository.GetUser(user.Value);

      if (Attachments != null)
        foreach (Attachment attachments in Attachments)
          attachments.SetNonOwnedAssociations(repository);
    }
  }
}

