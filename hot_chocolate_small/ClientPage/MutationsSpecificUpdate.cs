using System;
using System.Collections.Generic;
using System.Linq;

using HotChocolate;
using HotChocolate.Types;

using x10.hotchoc.ClientPage.Entities;
using x10.hotchoc.ClientPage.Repositories;

namespace x10.hotchoc.ClientPage {
  [ExtendObjectType(Name = "Mutation")]
  public partial class Mutations {

    #region CompanyEntityForm
    /// <summary>
    /// Input Data Type for CompanyEntityFormUpdateCompanyEntity Mutation
    /// </summary>
    public class CompanyEntityFormCompanyEntity : Base {
      [GraphQLNonNullType]
      public string? LegalName { get; set; }
      [GraphQLNonNullType]
      public string? DoingBusinessAs { get; set; }
      public CompanyEntityTypeEnum? CompanyType { get; set; }
      public Country CountryOfBusinessRegistration { get; set; }
      [GraphQLNonNullType]
      public string? StateOfBusinessRegistration { get; set; }
      [GraphQLNonNullType]
      public string? UsTaxId { get; set; }
      [GraphQLNonNullType]
      public Address MailingAddress { get; set; }
      [GraphQLNonNullType]
      public bool MailingAddressIsPhysicalAddress { get; set; }
      public Address PhysicalAddress { get; set; }
      [GraphQLNonNullType]
      public string? UsFccNumber { get; set; }
      [GraphQLNonNullType]
      public string? EoriNumber { get; set; }
      [GraphQLNonNullType]
      public string? UsciNumber { get; set; }
      [GraphQLNonNullType]
      public List<VatNumber>? VatNumbers { get; set; }
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
      [GraphQLNonNullType]
      public List<HkspPartnerUse>? HkspPartners { get; set; }
      public NetsuiteVendor NetsuiteVendorId { get; set; }
      public VendorCategoryEnum? VendorCategory { get; set; }
    }

    /// <summary>
    /// Update mutation for the CompanyEntityForm component
    /// </summary>
    public virtual CompanyEntity CompanyEntityFormUpdateCompanyEntity(
      CompanyEntityFormCompanyEntity data,
      [Service] IRepository repository) {
        throw new NotImplementedException("Manually override this method");
    }
    #endregion

  }
}
