using System;
using System.Collections.Generic;
using System.Linq;

using HotChocolate;
using HotChocolate.Types;

using x10.hotchoc.ClientPage.Entities;
using x10.hotchoc.ClientPage.Repositories;

namespace x10.hotchoc.ClientPage {
  [ExtendObjectType(Name = "Mutation")]
  public class Mutations {

    #region NetsuiteVendor
    /// <summary>
    /// Creates a new NetsuiteVendor or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateNetsuiteVendor(
        string id,
        string name,
        [Service] IRepository repository) {

      NetsuiteVendor netsuiteVendor = new NetsuiteVendor() {
        Name = name,
      };

      int dbid = repository.AddOrUpdateNetsuiteVendor(IdUtils.FromRelayId(id), netsuiteVendor);
      return IdUtils.ToRelayId<NetsuiteVendor>(dbid);
    }
    #endregion

    #region Company
    /// <summary>
    /// Creates a new Company or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateCompany(
        string id,
        string? website,
        CompanyEntity primaryEntity,
        IEnumerable<CompanyEntity> entities,
        Contact primaryContact,
        IEnumerable<User> users,
        [Service] IRepository repository) {

      Company company = new Company() {
        Website = website,
        PrimaryEntity = primaryEntity,
        Entities = entities.ToList(),
        PrimaryContact = primaryContact,
        Users = users.ToList(),
      };

      int dbid = repository.AddOrUpdateCompany(IdUtils.FromRelayId(id), company);
      return IdUtils.ToRelayId<Company>(dbid);
    }
    #endregion

    #region StateOrProvince
    /// <summary>
    /// Creates a new StateOrProvince or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateStateOrProvince(
        string id,
        string name,
        [Service] IRepository repository) {

      StateOrProvince stateOrProvince = new StateOrProvince() {
        Name = name,
      };

      int dbid = repository.AddOrUpdateStateOrProvince(IdUtils.FromRelayId(id), stateOrProvince);
      return IdUtils.ToRelayId<StateOrProvince>(dbid);
    }
    #endregion

    #region CtpatReview
    /// <summary>
    /// Creates a new CtpatReview or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateCtpatReview(
        string id,
        bool complianceScreenRequired,
        CtpatReviewStatusEnum status,
        DateTime? expiresAt,
        string complianceContactEmail,
        [Service] IRepository repository) {

      CtpatReview ctpatReview = new CtpatReview() {
        ComplianceScreenRequired = complianceScreenRequired,
        Status = status,
        ExpiresAt = expiresAt,
        ComplianceContactEmail = complianceContactEmail,
      };

      int dbid = repository.AddOrUpdateCtpatReview(IdUtils.FromRelayId(id), ctpatReview);
      return IdUtils.ToRelayId<CtpatReview>(dbid);
    }
    #endregion

    #region CompanyEntity
    /// <summary>
    /// Creates a new CompanyEntity or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateCompanyEntity(
        string id,
        string legalName,
        CompanyEntityTypeEnum companyType,
        bool isPrimary,
        bool dgDisclaimerAgreed,
        bool mailingAddressIsPhysicalAddress,
        string? brBlCompanyName,
        bool isArchived,
        string? brBlRegistrationNumber,
        string? brBlAddress,
        string? brBlLegalRepChinese,
        string? brBlLegalRepPinyin,
        string? usFccNumber,
        string? eoriNumber,
        string? usciNumber,
        string? agentIataCode,
        string? hkRaNumber,
        VendorCategoryEnum? vendorCategory,
        Address mailingAddress,
        Address? physicalAddress,
        IEnumerable<VatNumber> vatNumbers,
        string? netsuiteVendorIdId,
        CtpatReview? ctpatReview,
        IEnumerable<Document> documents,
        string? invoiceCurrencyDefaultId,
        [Service] IRepository repository) {

      CompanyEntity companyEntity = new CompanyEntity() {
        LegalName = legalName,
        CompanyType = companyType,
        IsPrimary = isPrimary,
        DgDisclaimerAgreed = dgDisclaimerAgreed,
        MailingAddressIsPhysicalAddress = mailingAddressIsPhysicalAddress,
        BrBlCompanyName = brBlCompanyName,
        IsArchived = isArchived,
        BrBlRegistrationNumber = brBlRegistrationNumber,
        BrBlAddress = brBlAddress,
        BrBlLegalRepChinese = brBlLegalRepChinese,
        BrBlLegalRepPinyin = brBlLegalRepPinyin,
        UsFccNumber = usFccNumber,
        EoriNumber = eoriNumber,
        UsciNumber = usciNumber,
        AgentIataCode = agentIataCode,
        HkRaNumber = hkRaNumber,
        VendorCategory = vendorCategory,
        MailingAddress = mailingAddress,
        PhysicalAddress = physicalAddress,
        VatNumbers = vatNumbers.ToList(),
        NetsuiteVendorId = repository.GetNetsuiteVendor(IdUtils.FromRelayIdMandatory(netsuiteVendorIdId)),
        CtpatReview = ctpatReview,
        Documents = documents.ToList(),
        InvoiceCurrencyDefault = repository.GetCurrency(IdUtils.FromRelayIdMandatory(invoiceCurrencyDefaultId)),
      };

      int dbid = repository.AddOrUpdateCompanyEntity(IdUtils.FromRelayId(id), companyEntity);
      return IdUtils.ToRelayId<CompanyEntity>(dbid);
    }
    #endregion

    #region Currency
    /// <summary>
    /// Creates a new Currency or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateCurrency(
        string id,
        string symbol,
        string name,
        [Service] IRepository repository) {

      Currency currency = new Currency() {
        Symbol = symbol,
        Name = name,
      };

      int dbid = repository.AddOrUpdateCurrency(IdUtils.FromRelayId(id), currency);
      return IdUtils.ToRelayId<Currency>(dbid);
    }
    #endregion

    #region Address
    /// <summary>
    /// Creates a new Address or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateAddress(
        string id,
        string theAddress,
        string? theAddress2,
        string city,
        string? postalCode,
        bool verified,
        string countryId,
        string stateOrProvinceId,
        [Service] IRepository repository) {

      Address address = new Address() {
        TheAddress = theAddress,
        TheAddress2 = theAddress2,
        City = city,
        PostalCode = postalCode,
        Verified = verified,
        Country = repository.GetCountry(IdUtils.FromRelayIdMandatory(countryId)),
        StateOrProvince = repository.GetStateOrProvince(IdUtils.FromRelayIdMandatory(stateOrProvinceId)),
      };

      int dbid = repository.AddOrUpdateAddress(IdUtils.FromRelayId(id), address);
      return IdUtils.ToRelayId<Address>(dbid);
    }
    #endregion

    #region User
    /// <summary>
    /// Creates a new User or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateUser(
        string id,
        string firstName,
        string lastName,
        string? phone,
        string email,
        [Service] IRepository repository) {

      User user = new User() {
        FirstName = firstName,
        LastName = lastName,
        Phone = phone,
        Email = email,
      };

      int dbid = repository.AddOrUpdateUser(IdUtils.FromRelayId(id), user);
      return IdUtils.ToRelayId<User>(dbid);
    }
    #endregion

    #region Document
    /// <summary>
    /// Creates a new Document or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateDocument(
        string id,
        string name,
        string fileName,
        DocumentTypeEnum documentType,
        [Service] IRepository repository) {

      Document document = new Document() {
        Name = name,
        FileName = fileName,
        DocumentType = documentType,
      };

      int dbid = repository.AddOrUpdateDocument(IdUtils.FromRelayId(id), document);
      return IdUtils.ToRelayId<Document>(dbid);
    }
    #endregion

    #region Client
    /// <summary>
    /// Creates a new Client or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateClient(
        string id,
        string? salesforceAccountRef,
        string? referredBy,
        ClientStatusEnum status,
        ClientSegmentEnum segment,
        ClientPurchasingBehaviorEnum? purchasingBehavior,
        ClientPrimaryShipmentRoleEnum primaryShipmentRole,
        Company company,
        Contact primaryContact,
        [Service] IRepository repository) {

      Client client = new Client() {
        SalesforceAccountRef = salesforceAccountRef,
        ReferredBy = referredBy,
        Status = status,
        Segment = segment,
        PurchasingBehavior = purchasingBehavior,
        PrimaryShipmentRole = primaryShipmentRole,
        Company = company,
        PrimaryContact = primaryContact,
      };

      int dbid = repository.AddOrUpdateClient(IdUtils.FromRelayId(id), client);
      return IdUtils.ToRelayId<Client>(dbid);
    }
    #endregion

    #region VatNumber
    /// <summary>
    /// Creates a new VatNumber or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateVatNumber(
        string id,
        string number,
        string countryRegionId,
        [Service] IRepository repository) {

      VatNumber vatNumber = new VatNumber() {
        Number = number,
        CountryRegion = repository.GetCountry(IdUtils.FromRelayIdMandatory(countryRegionId)),
      };

      int dbid = repository.AddOrUpdateVatNumber(IdUtils.FromRelayId(id), vatNumber);
      return IdUtils.ToRelayId<VatNumber>(dbid);
    }
    #endregion

    #region Country
    /// <summary>
    /// Creates a new Country or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateCountry(
        string id,
        string code,
        string name,
        IEnumerable<StateOrProvince> subRegions,
        [Service] IRepository repository) {

      Country country = new Country() {
        Code = code,
        Name = name,
        SubRegions = subRegions.ToList(),
      };

      int dbid = repository.AddOrUpdateCountry(IdUtils.FromRelayId(id), country);
      return IdUtils.ToRelayId<Country>(dbid);
    }
    #endregion

    #region Contact
    /// <summary>
    /// Creates a new Contact or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateContact(
        string id,
        string firstName,
        string lastName,
        string? phone,
        string email,
        [Service] IRepository repository) {

      Contact contact = new Contact() {
        FirstName = firstName,
        LastName = lastName,
        Phone = phone,
        Email = email,
      };

      int dbid = repository.AddOrUpdateContact(IdUtils.FromRelayId(id), contact);
      return IdUtils.ToRelayId<Contact>(dbid);
    }
    #endregion

    #region CompanyEntityCountryService
    /// <summary>
    /// Creates a new CompanyEntityCountryService or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateCompanyEntityCountryService(
        string id,
        bool importCustoms,
        bool exportCustoms,
        string countryId,
        [Service] IRepository repository) {

      CompanyEntityCountryService companyEntityCountryService = new CompanyEntityCountryService() {
        ImportCustoms = importCustoms,
        ExportCustoms = exportCustoms,
        Country = repository.GetCountry(IdUtils.FromRelayIdMandatory(countryId)),
      };

      int dbid = repository.AddOrUpdateCompanyEntityCountryService(IdUtils.FromRelayId(id), companyEntityCountryService);
      return IdUtils.ToRelayId<CompanyEntityCountryService>(dbid);
    }
    #endregion

  }
}
