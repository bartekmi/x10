using System.Collections.Generic;

using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

using x10.hotchoc.ClientPage.Entities;
using x10.hotchoc.ClientPage.Repositories;

namespace x10.hotchoc.ClientPage {
  [ExtendObjectType(Name = "Query")]
  public class Queries {

    #region NetsuiteVendor
    /// <summary>
    /// Retrieve a NetsuiteVendor by id
    /// </summary>
    /// <param name="id">The id of the NetsuiteVendor.</param>
    /// <param name="repository"></param>
    /// <returns>The NetsuiteVendor.</returns>
    public NetsuiteVendor GetNetsuiteVendor(
        string id,
        [Service] IRepository repository) =>
          repository.GetNetsuiteVendor(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all NetsuiteVendors.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All NetsuiteVendors.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<NetsuiteVendor> GetNetsuiteVendors(
        [Service] IRepository repository) =>
          repository.GetNetsuiteVendors();
    #endregion

    #region Company
    /// <summary>
    /// Retrieve a Company by id
    /// </summary>
    /// <param name="id">The id of the Company.</param>
    /// <param name="repository"></param>
    /// <returns>The Company.</returns>
    public Company GetCompany(
        string id,
        [Service] IRepository repository) =>
          repository.GetCompany(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Companies.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Companies.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Company> GetCompanies(
        [Service] IRepository repository) =>
          repository.GetCompanies();
    #endregion

    #region StateOrProvince
    /// <summary>
    /// Retrieve a StateOrProvince by id
    /// </summary>
    /// <param name="id">The id of the StateOrProvince.</param>
    /// <param name="repository"></param>
    /// <returns>The StateOrProvince.</returns>
    public StateOrProvince GetStateOrProvince(
        string id,
        [Service] IRepository repository) =>
          repository.GetStateOrProvince(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all StateOrProvinces.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All StateOrProvinces.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<StateOrProvince> GetStateOrProvinces(
        [Service] IRepository repository) =>
          repository.GetStateOrProvinces();
    #endregion

    #region CtpatReview
    /// <summary>
    /// Retrieve a CtpatReview by id
    /// </summary>
    /// <param name="id">The id of the CtpatReview.</param>
    /// <param name="repository"></param>
    /// <returns>The CtpatReview.</returns>
    public CtpatReview GetCtpatReview(
        string id,
        [Service] IRepository repository) =>
          repository.GetCtpatReview(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all CtpatReviews.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All CtpatReviews.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<CtpatReview> GetCtpatReviews(
        [Service] IRepository repository) =>
          repository.GetCtpatReviews();
    #endregion

    #region CompanyEntity
    /// <summary>
    /// Retrieve a CompanyEntity by id
    /// </summary>
    /// <param name="id">The id of the CompanyEntity.</param>
    /// <param name="repository"></param>
    /// <returns>The CompanyEntity.</returns>
    public CompanyEntity GetCompanyEntity(
        string id,
        [Service] IRepository repository) =>
          repository.GetCompanyEntity(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all CompanyEntities.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All CompanyEntities.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<CompanyEntity> GetCompanyEntities(
        [Service] IRepository repository) =>
          repository.GetCompanyEntities();
    #endregion

    #region Currency
    /// <summary>
    /// Retrieve a Currency by id
    /// </summary>
    /// <param name="id">The id of the Currency.</param>
    /// <param name="repository"></param>
    /// <returns>The Currency.</returns>
    public Currency GetCurrency(
        string id,
        [Service] IRepository repository) =>
          repository.GetCurrency(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Currencies.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Currencies.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Currency> GetCurrencies(
        [Service] IRepository repository) =>
          repository.GetCurrencies();
    #endregion

    #region Address
    /// <summary>
    /// Retrieve a Address by id
    /// </summary>
    /// <param name="id">The id of the Address.</param>
    /// <param name="repository"></param>
    /// <returns>The Address.</returns>
    public Address GetAddress(
        string id,
        [Service] IRepository repository) =>
          repository.GetAddress(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Addresses.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Addresses.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Address> GetAddresses(
        [Service] IRepository repository) =>
          repository.GetAddresses();
    #endregion

    #region User
    /// <summary>
    /// Retrieve a User by id
    /// </summary>
    /// <param name="id">The id of the User.</param>
    /// <param name="repository"></param>
    /// <returns>The User.</returns>
    public User GetUser(
        string id,
        [Service] IRepository repository) =>
          repository.GetUser(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Users.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Users.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<User> GetUsers(
        [Service] IRepository repository) =>
          repository.GetUsers();
    #endregion

    #region Document
    /// <summary>
    /// Retrieve a Document by id
    /// </summary>
    /// <param name="id">The id of the Document.</param>
    /// <param name="repository"></param>
    /// <returns>The Document.</returns>
    public Document GetDocument(
        string id,
        [Service] IRepository repository) =>
          repository.GetDocument(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Documents.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Documents.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Document> GetDocuments(
        [Service] IRepository repository) =>
          repository.GetDocuments();
    #endregion

    #region Client
    /// <summary>
    /// Retrieve a Client by id
    /// </summary>
    /// <param name="id">The id of the Client.</param>
    /// <param name="repository"></param>
    /// <returns>The Client.</returns>
    public Client GetClient(
        string id,
        [Service] IRepository repository) =>
          repository.GetClient(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Clients.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Clients.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Client> GetClients(
        [Service] IRepository repository) =>
          repository.GetClients();
    #endregion

    #region VatNumber
    /// <summary>
    /// Retrieve a VatNumber by id
    /// </summary>
    /// <param name="id">The id of the VatNumber.</param>
    /// <param name="repository"></param>
    /// <returns>The VatNumber.</returns>
    public VatNumber GetVatNumber(
        string id,
        [Service] IRepository repository) =>
          repository.GetVatNumber(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all VatNumbers.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All VatNumbers.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<VatNumber> GetVatNumbers(
        [Service] IRepository repository) =>
          repository.GetVatNumbers();
    #endregion

    #region Country
    /// <summary>
    /// Retrieve a Country by id
    /// </summary>
    /// <param name="id">The id of the Country.</param>
    /// <param name="repository"></param>
    /// <returns>The Country.</returns>
    public Country GetCountry(
        string id,
        [Service] IRepository repository) =>
          repository.GetCountry(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Countries.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Countries.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Country> GetCountries(
        [Service] IRepository repository) =>
          repository.GetCountries();
    #endregion

    #region Contact
    /// <summary>
    /// Retrieve a Contact by id
    /// </summary>
    /// <param name="id">The id of the Contact.</param>
    /// <param name="repository"></param>
    /// <returns>The Contact.</returns>
    public Contact GetContact(
        string id,
        [Service] IRepository repository) =>
          repository.GetContact(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Contacts.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Contacts.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Contact> GetContacts(
        [Service] IRepository repository) =>
          repository.GetContacts();
    #endregion

    #region CompanyEntityCountryService
    /// <summary>
    /// Retrieve a CompanyEntityCountryService by id
    /// </summary>
    /// <param name="id">The id of the CompanyEntityCountryService.</param>
    /// <param name="repository"></param>
    /// <returns>The CompanyEntityCountryService.</returns>
    public CompanyEntityCountryService GetCompanyEntityCountryService(
        string id,
        [Service] IRepository repository) =>
          repository.GetCompanyEntityCountryService(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all CompanyEntityCountryServices.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All CompanyEntityCountryServices.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<CompanyEntityCountryService> GetCompanyEntityCountryServices(
        [Service] IRepository repository) =>
          repository.GetCompanyEntityCountryServices();
    #endregion

  }
}
