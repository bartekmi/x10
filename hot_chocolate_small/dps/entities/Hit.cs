using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps.Entities {
  // Enums
  public enum UrgencyEnum {
    Low,
    Medium,
    High,
  }


  /// <summary>
  /// A "hit" on a User or Company Entity as reported by LexisNexis
  /// </summary>
  public class Hit : Base {
    // Regular Attributes
    public UrgencyEnum? Urgency { get; set; }
    public HitStatusEnum? Status { get; set; }
    public ReasonForClearanceEnum? ReasonForClearance { get; set; }
    [GraphQLNonNullType]
    public string? Notes { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "Hit: " + DbidHotChoc; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    public CompanyEntity? CompanyEntity { get; set; }
    public User? User { get; set; }
    [GraphQLNonNullType]
    public List<DpsAttachment>? Attachments { get; set; }
    [GraphQLNonNullType]
    public List<MatchInfo>? Matches { get; set; }
    [GraphQLNonNullType]
    public List<Shipment>? Shipments { get; set; }
    [GraphQLNonNullType]
    public List<Quote>? Quotes { get; set; }
    [GraphQLNonNullType]
    public List<Booking>? Bookings { get; set; }
    [GraphQLNonNullType]
    public List<DpsMessage>? Messages { get; set; }
    [GraphQLNonNullType]
    public List<OldHit>? OldHits { get; set; }
    [GraphQLNonNullType]
    public WhitelistDuration? WhitelistDays { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      Attachments?.ForEach(x => x.EnsureUniqueDbid());
      Matches?.ForEach(x => x.EnsureUniqueDbid());
      Shipments?.ForEach(x => x.EnsureUniqueDbid());
      Quotes?.ForEach(x => x.EnsureUniqueDbid());
      Bookings?.ForEach(x => x.EnsureUniqueDbid());
      Messages?.ForEach(x => x.EnsureUniqueDbid());
      OldHits?.ForEach(x => x.EnsureUniqueDbid());
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      int? companyEntity = IdUtils.FromFrontEndId(CompanyEntity?.Id);
      CompanyEntity = companyEntity == null ? null : repository.GetCompanyEntity(companyEntity.Value);

      int? user = IdUtils.FromFrontEndId(User?.Id);
      User = user == null ? null : repository.GetUser(user.Value);

      if (Attachments != null)
        foreach (DpsAttachment attachments in Attachments)
          attachments.SetNonOwnedAssociations(repository);

      if (Matches != null)
        foreach (MatchInfo matches in Matches)
          matches.SetNonOwnedAssociations(repository);

      if (Shipments != null)
        foreach (Shipment shipments in Shipments)
          shipments.SetNonOwnedAssociations(repository);

      if (Quotes != null)
        foreach (Quote quotes in Quotes)
          quotes.SetNonOwnedAssociations(repository);

      if (Bookings != null)
        foreach (Booking bookings in Bookings)
          bookings.SetNonOwnedAssociations(repository);

      if (Messages != null)
        foreach (DpsMessage messages in Messages)
          messages.SetNonOwnedAssociations(repository);

      if (OldHits != null)
        foreach (OldHit oldHits in OldHits)
          oldHits.SetNonOwnedAssociations(repository);

      int? whitelistDays = IdUtils.FromFrontEndId(WhitelistDays?.Id);
      WhitelistDays = whitelistDays == null ? null : repository.GetWhitelistDuration(whitelistDays.Value);
    }
  }
}

