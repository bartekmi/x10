using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  // Enums
  public enum PriorityEnum {
    Low,
    Middle,
    High,
  }

  public enum HitStatusEnum {
    Unresolved,
    Cleared,
    Denied,
  }

  public enum ReasonForCleranceEnum {
    PersonNotEntity,
    PartialName,
    PartialAddress,
    Other,
  }


  /// <summary>
  /// A "hit" on a User or Company Entity as reported by LexisNexis
  /// </summary>
  public class Hit : Base {
    // Regular Attributes
    public PriorityEnum? Priority { get; set; }
    public HitStatusEnum? Status { get; set; }
    [GraphQLNonNullType]
    public ReasonForCleranceEnum? ReasonForClearance { get; set; }
    [GraphQLNonNullType]
    public string? Notes { get; set; }
    [GraphQLNonNullType]
    public int? WhitelistTime { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "Hit: " + Dbid; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public CompanyEntity? CompanyEntity { get; set; }
    public User? User { get; set; }
    [GraphQLNonNullType]
    public List<Attachment>? Attachments { get; set; }
    [GraphQLNonNullType]
    public List<MatchInfo>? Matches { get; set; }
    [GraphQLNonNullType]
    public List<SuggestedResource>? Resources { get; set; }
    [GraphQLNonNullType]
    public List<Shipment>? Shipments { get; set; }
    [GraphQLNonNullType]
    public List<Message>? Messages { get; set; }
    [GraphQLNonNullType]
    public List<OldHit>? OldHits { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      Attachments?.ForEach(x => x.EnsureUniqueDbid());
      Matches?.ForEach(x => x.EnsureUniqueDbid());
      Resources?.ForEach(x => x.EnsureUniqueDbid());
      Shipments?.ForEach(x => x.EnsureUniqueDbid());
      Messages?.ForEach(x => x.EnsureUniqueDbid());
      OldHits?.ForEach(x => x.EnsureUniqueDbid());
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? companyEntity = IdUtils.FromRelayId(CompanyEntity?.Id);
      CompanyEntity = companyEntity == null ? null : repository.GetCompanyEntity(companyEntity.Value);

      int? user = IdUtils.FromRelayId(User?.Id);
      User = user == null ? null : repository.GetUser(user.Value);

      if (Attachments != null)
        foreach (Attachment attachments in Attachments)
          attachments.SetNonOwnedAssociations(repository);

      if (Matches != null)
        foreach (MatchInfo matches in Matches)
          matches.SetNonOwnedAssociations(repository);

      if (Resources != null)
        foreach (SuggestedResource resources in Resources)
          resources.SetNonOwnedAssociations(repository);

      if (Shipments != null)
        foreach (Shipment shipments in Shipments)
          shipments.SetNonOwnedAssociations(repository);

      if (Messages != null)
        foreach (Message messages in Messages)
          messages.SetNonOwnedAssociations(repository);

      if (OldHits != null)
        foreach (OldHit oldHits in OldHits)
          oldHits.SetNonOwnedAssociations(repository);
    }
  }
}

