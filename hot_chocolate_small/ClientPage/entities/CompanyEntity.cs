using System;
using System.Collections.Generic;

using HotChocolate;

using x10.hotchoc.ClientPage.Repositories;

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

  public enum HkspFlexportEnum {
    KnownConsignor,
    AcccountConsignor,
    Unknown,
  }


  /// <summary>
  /// A legal entity - typically a corporation - which belongs to a [Company]
  /// </summary>
  public class CompanyEntity : Base {
    // Regular Attributes
    [GraphQLNonNullType]
    public string? LegalName { get; set; }
    [GraphQLNonNullType]
    public string? DoingBusinessAs { get; set; }
    public int? CoreId { get; set; }
    [GraphQLNonNullType]
    public string? AdminEmail { get; set; }
    [GraphQLNonNullType]
    public CompanyEntityTypeEnum? CompanyType { get; set; }
    [GraphQLNonNullType]
    public string? StateOfBusinessRegistration { get; set; }
    [GraphQLNonNullType]
    public string? UsTaxId { get; set; }
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
    public HkspFlexportEnum? HkspFlexport { get; set; }
    [GraphQLNonNullType]
    public string? HkspKnownConsignorNumber { get; set; }
    public DateTime? HkspStatusExpirationDate { get; set; }
    [GraphQLNonNullType]
    public string? HkspKcResponsiblePerson { get; set; }
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
    public CtpatReview? CtpatReview { get; set; }
    [GraphQLNonNullType]
    public List<Document>? Documents { get; set; }
    public Country? CountryOfBusinessRegistration { get; set; }
    public Currency? InvoiceCurrencyDefault { get; set; }
    [GraphQLNonNullType]
    public List<HkspPartnerUse>? HkspPartners { get; set; }

    public override void EnsureUniqueDbid() {
      base.EnsureUniqueDbid();
      MailingAddress?.EnsureUniqueDbid();
      PhysicalAddress?.EnsureUniqueDbid();
      VatNumbers?.ForEach(x => x.EnsureUniqueDbid());
      CtpatReview?.EnsureUniqueDbid();
      Documents?.ForEach(x => x.EnsureUniqueDbid());
      HkspPartners?.ForEach(x => x.EnsureUniqueDbid());
    }

    internal override void SetNonOwnedAssociations(IRepository repository) {
      base.SetNonOwnedAssociations(repository);

      MailingAddress?.SetNonOwnedAssociations(repository);

      PhysicalAddress?.SetNonOwnedAssociations(repository);

      if (VatNumbers != null)
        foreach (VatNumber vatNumbers in VatNumbers)
          vatNumbers.SetNonOwnedAssociations(repository);

      int? netsuiteVendorId = IdUtils.FromRelayId(NetsuiteVendorId?.Id);
      NetsuiteVendorId = netsuiteVendorId == null ? null : repository.GetNetsuiteVendor(netsuiteVendorId.Value);

      CtpatReview?.SetNonOwnedAssociations(repository);

      if (Documents != null)
        foreach (Document documents in Documents)
          documents.SetNonOwnedAssociations(repository);

      int? countryOfBusinessRegistration = IdUtils.FromRelayId(CountryOfBusinessRegistration?.Id);
      CountryOfBusinessRegistration = countryOfBusinessRegistration == null ? null : repository.GetCountry(countryOfBusinessRegistration.Value);

      int? invoiceCurrencyDefault = IdUtils.FromRelayId(InvoiceCurrencyDefault?.Id);
      InvoiceCurrencyDefault = invoiceCurrencyDefault == null ? null : repository.GetCurrency(invoiceCurrencyDefault.Value);

      if (HkspPartners != null)
        foreach (HkspPartnerUse hkspPartners in HkspPartners)
          hkspPartners.SetNonOwnedAssociations(repository);
    }
  }
}

