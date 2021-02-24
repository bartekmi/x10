using System;
using System.Collections.Generic;
using System.Linq;

using x10.hotchoc.ClientPage.Entities;

namespace x10.hotchoc.ClientPage.Repositories {
  public class Repository : RepositoryBase, IRepository {
    private Dictionary<int, NetsuiteVendor> _netsuiteVendors = new Dictionary<int, NetsuiteVendor>();
    private Dictionary<int, HkspPartner> _hkspPartners = new Dictionary<int, HkspPartner>();
    private Dictionary<int, Company> _companies = new Dictionary<int, Company>();
    private Dictionary<int, StateOrProvince> _stateOrProvinces = new Dictionary<int, StateOrProvince>();
    private Dictionary<int, CtpatReview> _ctpatReviews = new Dictionary<int, CtpatReview>();
    private Dictionary<int, HkspPartnerUse> _hkspPartnerUses = new Dictionary<int, HkspPartnerUse>();
    private Dictionary<int, CompanyEntity> _companyEntities = new Dictionary<int, CompanyEntity>();
    private Dictionary<int, Currency> _currencies = new Dictionary<int, Currency>();
    private Dictionary<int, Address> _addresses = new Dictionary<int, Address>();
    private Dictionary<int, User> _users = new Dictionary<int, User>();
    private Dictionary<int, Document> _documents = new Dictionary<int, Document>();
    private Dictionary<int, Client> _clients = new Dictionary<int, Client>();
    private Dictionary<int, VatNumber> _vatNumbers = new Dictionary<int, VatNumber>();
    private Dictionary<int, Country> _countries = new Dictionary<int, Country>();
    private Dictionary<int, Contact> _contacts = new Dictionary<int, Contact>();
    private Dictionary<int, CompanyEntityCountryService> _companyEntityCountryServices = new Dictionary<int, CompanyEntityCountryService>();

    public override IEnumerable<Type> Types() {
      return new Type[] {
        typeof(Queries),
        typeof(Mutations),
        typeof(NetsuiteVendor),
        typeof(HkspPartner),
        typeof(Company),
        typeof(StateOrProvince),
        typeof(CtpatReview),
        typeof(HkspPartnerUse),
        typeof(CompanyEntity),
        typeof(Currency),
        typeof(Address),
        typeof(User),
        typeof(Document),
        typeof(Client),
        typeof(VatNumber),
        typeof(Country),
        typeof(Contact),
        typeof(CompanyEntityCountryService),
      };
    }

    public override void Add(PrimordialEntityBase instance) {
      int id = instance.Dbid;

      if (instance is NetsuiteVendor netsuiteVendor) _netsuiteVendors[id] = netsuiteVendor;
      if (instance is HkspPartner hkspPartner) _hkspPartners[id] = hkspPartner;
      if (instance is Company company) _companies[id] = company;
      if (instance is StateOrProvince stateOrProvince) _stateOrProvinces[id] = stateOrProvince;
      if (instance is CtpatReview ctpatReview) _ctpatReviews[id] = ctpatReview;
      if (instance is HkspPartnerUse hkspPartnerUse) _hkspPartnerUses[id] = hkspPartnerUse;
      if (instance is CompanyEntity companyEntity) _companyEntities[id] = companyEntity;
      if (instance is Currency currency) _currencies[id] = currency;
      if (instance is Address address) _addresses[id] = address;
      if (instance is User user) _users[id] = user;
      if (instance is Document document) _documents[id] = document;
      if (instance is Client client) _clients[id] = client;
      if (instance is VatNumber vatNumber) _vatNumbers[id] = vatNumber;
      if (instance is Country country) _countries[id] = country;
      if (instance is Contact contact) _contacts[id] = contact;
      if (instance is CompanyEntityCountryService companyEntityCountryService) _companyEntityCountryServices[id] = companyEntityCountryService;
    }

    #region NetsuiteVendors
    public IQueryable<NetsuiteVendor> GetNetsuiteVendors() => _netsuiteVendors.Values.AsQueryable();
    public NetsuiteVendor GetNetsuiteVendor(int id) { return _netsuiteVendors[id]; }
    public int AddOrUpdateNetsuiteVendor(int? dbid, NetsuiteVendor netsuiteVendor) {
      return RepositoryUtils.AddOrUpdate(dbid, netsuiteVendor, _netsuiteVendors);
    }
    #endregion

    #region HkspPartners
    public IQueryable<HkspPartner> GetHkspPartners() => _hkspPartners.Values.AsQueryable();
    public HkspPartner GetHkspPartner(int id) { return _hkspPartners[id]; }
    public int AddOrUpdateHkspPartner(int? dbid, HkspPartner hkspPartner) {
      return RepositoryUtils.AddOrUpdate(dbid, hkspPartner, _hkspPartners);
    }
    #endregion

    #region Companies
    public IQueryable<Company> GetCompanies() => _companies.Values.AsQueryable();
    public Company GetCompany(int id) { return _companies[id]; }
    public int AddOrUpdateCompany(int? dbid, Company company) {
      return RepositoryUtils.AddOrUpdate(dbid, company, _companies);
    }
    #endregion

    #region StateOrProvinces
    public IQueryable<StateOrProvince> GetStateOrProvinces() => _stateOrProvinces.Values.AsQueryable();
    public StateOrProvince GetStateOrProvince(int id) { return _stateOrProvinces[id]; }
    public int AddOrUpdateStateOrProvince(int? dbid, StateOrProvince stateOrProvince) {
      return RepositoryUtils.AddOrUpdate(dbid, stateOrProvince, _stateOrProvinces);
    }
    #endregion

    #region CtpatReviews
    public IQueryable<CtpatReview> GetCtpatReviews() => _ctpatReviews.Values.AsQueryable();
    public CtpatReview GetCtpatReview(int id) { return _ctpatReviews[id]; }
    public int AddOrUpdateCtpatReview(int? dbid, CtpatReview ctpatReview) {
      return RepositoryUtils.AddOrUpdate(dbid, ctpatReview, _ctpatReviews);
    }
    #endregion

    #region HkspPartnerUses
    public IQueryable<HkspPartnerUse> GetHkspPartnerUses() => _hkspPartnerUses.Values.AsQueryable();
    public HkspPartnerUse GetHkspPartnerUse(int id) { return _hkspPartnerUses[id]; }
    public int AddOrUpdateHkspPartnerUse(int? dbid, HkspPartnerUse hkspPartnerUse) {
      return RepositoryUtils.AddOrUpdate(dbid, hkspPartnerUse, _hkspPartnerUses);
    }
    #endregion

    #region CompanyEntities
    public IQueryable<CompanyEntity> GetCompanyEntities() => _companyEntities.Values.AsQueryable();
    public CompanyEntity GetCompanyEntity(int id) { return _companyEntities[id]; }
    public int AddOrUpdateCompanyEntity(int? dbid, CompanyEntity companyEntity) {
      return RepositoryUtils.AddOrUpdate(dbid, companyEntity, _companyEntities);
    }
    #endregion

    #region Currencies
    public IQueryable<Currency> GetCurrencies() => _currencies.Values.AsQueryable();
    public Currency GetCurrency(int id) { return _currencies[id]; }
    public int AddOrUpdateCurrency(int? dbid, Currency currency) {
      return RepositoryUtils.AddOrUpdate(dbid, currency, _currencies);
    }
    #endregion

    #region Addresses
    public IQueryable<Address> GetAddresses() => _addresses.Values.AsQueryable();
    public Address GetAddress(int id) { return _addresses[id]; }
    public int AddOrUpdateAddress(int? dbid, Address address) {
      return RepositoryUtils.AddOrUpdate(dbid, address, _addresses);
    }
    #endregion

    #region Users
    public IQueryable<User> GetUsers() => _users.Values.AsQueryable();
    public User GetUser(int id) { return _users[id]; }
    public int AddOrUpdateUser(int? dbid, User user) {
      return RepositoryUtils.AddOrUpdate(dbid, user, _users);
    }
    #endregion

    #region Documents
    public IQueryable<Document> GetDocuments() => _documents.Values.AsQueryable();
    public Document GetDocument(int id) { return _documents[id]; }
    public int AddOrUpdateDocument(int? dbid, Document document) {
      return RepositoryUtils.AddOrUpdate(dbid, document, _documents);
    }
    #endregion

    #region Clients
    public IQueryable<Client> GetClients() => _clients.Values.AsQueryable();
    public Client GetClient(int id) { return _clients[id]; }
    public int AddOrUpdateClient(int? dbid, Client client) {
      return RepositoryUtils.AddOrUpdate(dbid, client, _clients);
    }
    #endregion

    #region VatNumbers
    public IQueryable<VatNumber> GetVatNumbers() => _vatNumbers.Values.AsQueryable();
    public VatNumber GetVatNumber(int id) { return _vatNumbers[id]; }
    public int AddOrUpdateVatNumber(int? dbid, VatNumber vatNumber) {
      return RepositoryUtils.AddOrUpdate(dbid, vatNumber, _vatNumbers);
    }
    #endregion

    #region Countries
    public IQueryable<Country> GetCountries() => _countries.Values.AsQueryable();
    public Country GetCountry(int id) { return _countries[id]; }
    public int AddOrUpdateCountry(int? dbid, Country country) {
      return RepositoryUtils.AddOrUpdate(dbid, country, _countries);
    }
    #endregion

    #region Contacts
    public IQueryable<Contact> GetContacts() => _contacts.Values.AsQueryable();
    public Contact GetContact(int id) { return _contacts[id]; }
    public int AddOrUpdateContact(int? dbid, Contact contact) {
      return RepositoryUtils.AddOrUpdate(dbid, contact, _contacts);
    }
    #endregion

    #region CompanyEntityCountryServices
    public IQueryable<CompanyEntityCountryService> GetCompanyEntityCountryServices() => _companyEntityCountryServices.Values.AsQueryable();
    public CompanyEntityCountryService GetCompanyEntityCountryService(int id) { return _companyEntityCountryServices[id]; }
    public int AddOrUpdateCompanyEntityCountryService(int? dbid, CompanyEntityCountryService companyEntityCountryService) {
      return RepositoryUtils.AddOrUpdate(dbid, companyEntityCountryService, _companyEntityCountryServices);
    }
    #endregion

  }
}
