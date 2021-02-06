using System;
using System.Collections.Generic;

using HotChocolate;

namespace x10.hotchoc.ClientPage.Entities {
  // Enums
  public enum CompanyEntityTypeEnum {
    Corporation,
    Individual,
    LimitedLiabilityCompany,
    LimitedLiabilityPartnership,
    NonResidentCorporation,
    Partnership,
    SoleProprietorship,
    Unknown,
    UnlimitedLiabilityCorporation,
  }

  public enum VendorCategoryEnum {
    Unassigned,
    FreightUnapproved,
    FreightApproved,
    ArrivalNotice,
    ArrivalNoticeTerms,
    PassthroughApproved,
  }


  /// <summary>
  /// A legal entity - typically a corporation - which belongs to a <Company>
  /// </summary>
  public class CompanyEntity : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? LegalName { get; set; }
    public int? CoreId { get; set; }
    [GraphQLNonNullType]
    public CompanyEntityTypeEnum? CompanyType { get; set; }
    [GraphQLNonNullType]
    public bool IsPrimary { get; set; }
    [GraphQLNonNullType]
    public bool DgDisclaimerAgreed { get; set; }
    [GraphQLNonNullType]
    public bool MailingAddressIsPhysicalAddress { get; set; }
    [GraphQLNonNullType]
    public string? BrBlCompanyName { get; set; }
    [GraphQLNonNullType]
    public bool IsArchived { get; set; }
    [GraphQLNonNullType]
    public string? BrBlRegistrationNumber { get; set; }
    [GraphQLNonNullType]
    public string? BrBlAddress { get; set; }
    [GraphQLNonNullType]
    public string? BrBlLegalRepChinese { get; set; }
    [GraphQLNonNullType]
    public string? BrBlLegalRepPinyin { get; set; }
    [GraphQLNonNullType]
    public string? UsFccNumber { get; set; }
    [GraphQLNonNullType]
    public string? EoriNumber { get; set; }
    [GraphQLNonNullType]
    public string? UsciNumber { get; set; }
    [GraphQLNonNullType]
    public string? AgentIataCode { get; set; }
    [GraphQLNonNullType]
    public string? HkRaNumber { get; set; }
    public VendorCategoryEnum? VendorCategory { get; set; }

    // To String Representation
    [GraphQLNonNullType]
    public string? ToStringRepresentation {
      get { return "CompanyEntity: " + Dbid; }
      set { /* Needed to make Hot Chocolate happy */ }
    }

    // Associations
    [GraphQLNonNullType]
    public Address? MailingAddress { get; set; }
    public Address? PhysicalAddress { get; set; }
    [GraphQLNonNullType]
    public List<VatNumber>? VatNumbers { get; set; }
    public NetsuiteVendor? NetsuiteVendorId { get; set; }
    [GraphQLNonNullType]
    public List<Company>? InvoicedBy { get; set; }
    public Company? InvoicedByDefault { get; set; }
    [GraphQLNonNullType]
    public List<Currency>? InvoiceCurrencies { get; set; }
    public Currency? InvoiceCurrencyDefault { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      MailingAddress?.EnsureUniqueDbid();
      PhysicalAddress?.EnsureUniqueDbid();
      VatNumbers?.ForEach(x => x.EnsureUniqueDbid());
    }
  }
}

