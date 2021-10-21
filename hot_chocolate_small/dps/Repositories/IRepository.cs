using System;
using System.Collections.Generic;
using System.Linq;

using x10.hotchoc.dps.Entities;

namespace x10.hotchoc.dps.Repositories {
  public interface IRepository {
    // Queries
    Company GetCompany(int id);
    CompanyEntity GetCompanyEntity(int id);
    WhitelistDuration GetWhitelistDuration(int id);
    Hit GetHit(int id);
    Attachment GetAttachment(int id);
    MatchInfoSource GetMatchInfoSource(int id);
    Port GetPort(int id);
    MatchInfo GetMatchInfo(int id);
    OldHit GetOldHit(int id);
    User GetUser(int id);
    Shipment GetShipment(int id);
    Client GetClient(int id);
    AddressType GetAddressType(int id);
    Message GetMessage(int id);

    IQueryable<Company> GetCompanies();
    IQueryable<CompanyEntity> GetCompanyEntities();
    IQueryable<WhitelistDuration> GetWhitelistDurations();
    IQueryable<Hit> GetHits();
    IQueryable<Attachment> GetAttachments();
    IQueryable<MatchInfoSource> GetMatchInfoSources();
    IQueryable<Port> GetPorts();
    IQueryable<MatchInfo> GetMatchInfos();
    IQueryable<OldHit> GetOldHits();
    IQueryable<User> GetUsers();
    IQueryable<Shipment> GetShipments();
    IQueryable<Client> GetClients();
    IQueryable<AddressType> GetAddressTypes();
    IQueryable<Message> GetMessages();

    // Mutations
    int AddOrUpdateCompany(int? dbid, Company company);
    int AddOrUpdateCompanyEntity(int? dbid, CompanyEntity companyEntity);
    int AddOrUpdateWhitelistDuration(int? dbid, WhitelistDuration whitelistDuration);
    int AddOrUpdateHit(int? dbid, Hit hit);
    int AddOrUpdateAttachment(int? dbid, Attachment attachment);
    int AddOrUpdateMatchInfoSource(int? dbid, MatchInfoSource matchInfoSource);
    int AddOrUpdatePort(int? dbid, Port port);
    int AddOrUpdateMatchInfo(int? dbid, MatchInfo matchInfo);
    int AddOrUpdateOldHit(int? dbid, OldHit oldHit);
    int AddOrUpdateUser(int? dbid, User user);
    int AddOrUpdateShipment(int? dbid, Shipment shipment);
    int AddOrUpdateClient(int? dbid, Client client);
    int AddOrUpdateAddressType(int? dbid, AddressType addressType);
    int AddOrUpdateMessage(int? dbid, Message message);
  }
}
