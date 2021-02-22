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
    /// Creates a new NetsuiteVendor or updates an existing one, depending on the value of netsuiteVendor.id
    /// </summary>
    public string CreateOrUpdateNetsuiteVendor(
      NetsuiteVendor netsuiteVendor,
      [Service] IRepository repository) {
        netsuiteVendor.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateNetsuiteVendor(IdUtils.FromRelayId(netsuiteVendor.Id), netsuiteVendor);
        return IdUtils.ToRelayId<NetsuiteVendor>(dbid);
    }
    #endregion

    #region Company
    /// <summary>
    /// Creates a new Company or updates an existing one, depending on the value of company.id
    /// </summary>
    public string CreateOrUpdateCompany(
      Company company,
      [Service] IRepository repository) {
        company.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateCompany(IdUtils.FromRelayId(company.Id), company);
        return IdUtils.ToRelayId<Company>(dbid);
    }
    #endregion

    #region StateOrProvince
    /// <summary>
    /// Creates a new StateOrProvince or updates an existing one, depending on the value of stateOrProvince.id
    /// </summary>
    public string CreateOrUpdateStateOrProvince(
      StateOrProvince stateOrProvince,
      [Service] IRepository repository) {
        stateOrProvince.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateStateOrProvince(IdUtils.FromRelayId(stateOrProvince.Id), stateOrProvince);
        return IdUtils.ToRelayId<StateOrProvince>(dbid);
    }
    #endregion

    #region CtpatReview
    /// <summary>
    /// Creates a new CtpatReview or updates an existing one, depending on the value of ctpatReview.id
    /// </summary>
    public string CreateOrUpdateCtpatReview(
      CtpatReview ctpatReview,
      [Service] IRepository repository) {
        ctpatReview.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateCtpatReview(IdUtils.FromRelayId(ctpatReview.Id), ctpatReview);
        return IdUtils.ToRelayId<CtpatReview>(dbid);
    }
    #endregion

    #region CompanyEntity
    /// <summary>
    /// Creates a new CompanyEntity or updates an existing one, depending on the value of companyEntity.id
    /// </summary>
    public string CreateOrUpdateCompanyEntity(
      CompanyEntity companyEntity,
      [Service] IRepository repository) {
        companyEntity.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateCompanyEntity(IdUtils.FromRelayId(companyEntity.Id), companyEntity);
        return IdUtils.ToRelayId<CompanyEntity>(dbid);
    }
    #endregion

    #region Currency
    /// <summary>
    /// Creates a new Currency or updates an existing one, depending on the value of currency.id
    /// </summary>
    public string CreateOrUpdateCurrency(
      Currency currency,
      [Service] IRepository repository) {
        currency.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateCurrency(IdUtils.FromRelayId(currency.Id), currency);
        return IdUtils.ToRelayId<Currency>(dbid);
    }
    #endregion

    #region Address
    /// <summary>
    /// Creates a new Address or updates an existing one, depending on the value of address.id
    /// </summary>
    public string CreateOrUpdateAddress(
      Address address,
      [Service] IRepository repository) {
        address.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateAddress(IdUtils.FromRelayId(address.Id), address);
        return IdUtils.ToRelayId<Address>(dbid);
    }
    #endregion

    #region User
    /// <summary>
    /// Creates a new User or updates an existing one, depending on the value of user.id
    /// </summary>
    public string CreateOrUpdateUser(
      User user,
      [Service] IRepository repository) {
        user.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateUser(IdUtils.FromRelayId(user.Id), user);
        return IdUtils.ToRelayId<User>(dbid);
    }
    #endregion

    #region Document
    /// <summary>
    /// Creates a new Document or updates an existing one, depending on the value of document.id
    /// </summary>
    public string CreateOrUpdateDocument(
      Document document,
      [Service] IRepository repository) {
        document.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateDocument(IdUtils.FromRelayId(document.Id), document);
        return IdUtils.ToRelayId<Document>(dbid);
    }
    #endregion

    #region Client
    /// <summary>
    /// Creates a new Client or updates an existing one, depending on the value of client.id
    /// </summary>
    public string CreateOrUpdateClient(
      Client client,
      [Service] IRepository repository) {
        client.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateClient(IdUtils.FromRelayId(client.Id), client);
        return IdUtils.ToRelayId<Client>(dbid);
    }
    #endregion

    #region VatNumber
    /// <summary>
    /// Creates a new VatNumber or updates an existing one, depending on the value of vatNumber.id
    /// </summary>
    public string CreateOrUpdateVatNumber(
      VatNumber vatNumber,
      [Service] IRepository repository) {
        vatNumber.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateVatNumber(IdUtils.FromRelayId(vatNumber.Id), vatNumber);
        return IdUtils.ToRelayId<VatNumber>(dbid);
    }
    #endregion

    #region Country
    /// <summary>
    /// Creates a new Country or updates an existing one, depending on the value of country.id
    /// </summary>
    public string CreateOrUpdateCountry(
      Country country,
      [Service] IRepository repository) {
        country.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateCountry(IdUtils.FromRelayId(country.Id), country);
        return IdUtils.ToRelayId<Country>(dbid);
    }
    #endregion

    #region Contact
    /// <summary>
    /// Creates a new Contact or updates an existing one, depending on the value of contact.id
    /// </summary>
    public string CreateOrUpdateContact(
      Contact contact,
      [Service] IRepository repository) {
        contact.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateContact(IdUtils.FromRelayId(contact.Id), contact);
        return IdUtils.ToRelayId<Contact>(dbid);
    }
    #endregion

    #region CompanyEntityCountryService
    /// <summary>
    /// Creates a new CompanyEntityCountryService or updates an existing one, depending on the value of companyEntityCountryService.id
    /// </summary>
    public string CreateOrUpdateCompanyEntityCountryService(
      CompanyEntityCountryService companyEntityCountryService,
      [Service] IRepository repository) {
        companyEntityCountryService.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateCompanyEntityCountryService(IdUtils.FromRelayId(companyEntityCountryService.Id), companyEntityCountryService);
        return IdUtils.ToRelayId<CompanyEntityCountryService>(dbid);
    }
    #endregion

  }
}
