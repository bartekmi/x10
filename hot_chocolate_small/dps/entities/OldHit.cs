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
    public HitStatusEnum? Status { get; set; }
    public ReasonForClearanceEnum? ReasonForClearance { get; set; }
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
    public List<HitEdit>? ChangeLog { get; set; }
    [GraphQLNonNullType]
    public List<DpsAttachment>? Attachments { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      ChangeLog?.ForEach(x => x.EnsureUniqueDbid());
      Attachments?.ForEach(x => x.EnsureUniqueDbid());
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? resolvedBy = IdUtils.FromRelayId(ResolvedBy?.Id);
      ResolvedBy = resolvedBy == null ? null : repository.GetUser(resolvedBy.Value);

      if (ChangeLog != null)
        foreach (HitEdit changeLog in ChangeLog)
          changeLog.SetNonOwnedAssociations(repository);

      if (Attachments != null)
        foreach (DpsAttachment attachments in Attachments)
          attachments.SetNonOwnedAssociations(repository);
    }
  }
}

