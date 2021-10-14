using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  /// <summary>
  /// A "hit" on a User or Company Entity as reported by LexisNexis
  /// </summary>
  public class OldHit : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public HitStatusEnum? Status { get; set; }
    [GraphQLNonNullType]
    public ReasonForCleranceEnum? ReasonForClearance { get; set; }
    public DateTime? WhiteListUntil { get; set; }
    [GraphQLNonNullType]
    public string? Notes { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ResolutionTimestamp { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "OldHit: " + DbidHotChoc; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    public User? ResolvedBy { get; set; }
    [GraphQLNonNullType]
    public List<Attachment>? Attachments { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      Attachments?.ForEach(x => x.EnsureUniqueDbid());
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? resolvedBy = IdUtils.FromRelayId(ResolvedBy?.Id);
      ResolvedBy = resolvedBy == null ? null : repository.GetUser(resolvedBy.Value);

      if (Attachments != null)
        foreach (Attachment attachments in Attachments)
          attachments.SetNonOwnedAssociations(repository);
    }
  }
}

