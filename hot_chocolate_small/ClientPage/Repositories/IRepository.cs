using System;
using System.Collections.Generic;
using System.Linq;

using x10.hotchoc.ClientPage.Entities;

namespace x10.hotchoc.ClientPage.Repositories {
  public interface IRepository {
    // Queries
    NetsuiteVendor GetNetsuiteVendor(int id);
    Company GetCompany(int id);
    StateOrProvince GetStateOrProvince(int id);
    CtpatReview GetCtpatReview(int id);
    CompanyEntity GetCompanyEntity(int id);
    Currency GetCurrency(int id);
    Address GetAddress(int id);
    User GetUser(int id);
    Document GetDocument(int id);
    Client GetClient(int id);
    VatNumber GetVatNumber(int id);
    Country GetCountry(int id);
    Contact GetContact(int id);
    CompanyEntityCountryService GetCompanyEntityCountryService(int id);

    IQueryable<NetsuiteVendor> GetNetsuiteVendors();
    IQueryable<Company> GetCompanies();
    IQueryable<StateOrProvince> GetStateOrProvinces();
    IQueryable<CtpatReview> GetCtpatReviews();
    IQueryable<CompanyEntity> GetCompanyEntities();
    IQueryable<Currency> GetCurrencies();
    IQueryable<Address> GetAddresses();
    IQueryable<User> GetUsers();
    IQueryable<Document> GetDocuments();
    IQueryable<Client> GetClients();
    IQueryable<VatNumber> GetVatNumbers();
    IQueryable<Country> GetCountries();
    IQueryable<Contact> GetContacts();
    IQueryable<CompanyEntityCountryService> GetCompanyEntityCountryServices();

    // Mutations
    int AddOrUpdateNetsuiteVendor(int? dbid, NetsuiteVendor netsuiteVendor);
    int AddOrUpdateCompany(int? dbid, Company company);
    int AddOrUpdateStateOrProvince(int? dbid, StateOrProvince stateOrProvince);
    int AddOrUpdateCtpatReview(int? dbid, CtpatReview ctpatReview);
    int AddOrUpdateCompanyEntity(int? dbid, CompanyEntity companyEntity);
    int AddOrUpdateCurrency(int? dbid, Currency currency);
    int AddOrUpdateAddress(int? dbid, Address address);
    int AddOrUpdateUser(int? dbid, User user);
    int AddOrUpdateDocument(int? dbid, Document document);
    int AddOrUpdateClient(int? dbid, Client client);
    int AddOrUpdateVatNumber(int? dbid, VatNumber vatNumber);
    int AddOrUpdateCountry(int? dbid, Country country);
    int AddOrUpdateContact(int? dbid, Contact contact);
    int AddOrUpdateCompanyEntityCountryService(int? dbid, CompanyEntityCountryService companyEntityCountryService);
  }
}
