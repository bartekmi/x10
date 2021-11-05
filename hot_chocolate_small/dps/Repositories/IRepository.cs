using System;
using System.Collections.Generic;
using System.Linq;

using x10.hotchoc.dps.Entities;

namespace x10.hotchoc.dps.Repositories {
  public interface IRepository {
    // Queries
    Settings GetSettings(int id);
    HitEdit GetHitEdit(int id);
    Company GetCompany(int id);
    CompanyEntity GetCompanyEntity(int id);
    SettingsAutoAssignment GetSettingsAutoAssignment(int id);
    WhitelistDuration GetWhitelistDuration(int id);
    Hit GetHit(int id);
    MatchInfoSource GetMatchInfoSource(int id);
    Port GetPort(int id);
    MatchInfo GetMatchInfo(int id);
    DpsAttachment GetDpsAttachment(int id);
    OldHit GetOldHit(int id);
    User GetUser(int id);
    Shipment GetShipment(int id);
    Client GetClient(int id);
    AddressType GetAddressType(int id);
    DpsMessage GetDpsMessage(int id);

    IQueryable<Settings> GetSettingses();
    IQueryable<HitEdit> GetHitEdits();
    IQueryable<Company> GetCompanies();
    IQueryable<CompanyEntity> GetCompanyEntities();
    IQueryable<SettingsAutoAssignment> GetSettingsAutoAssignments();
    IQueryable<WhitelistDuration> GetWhitelistDurations();
    IQueryable<Hit> GetHits();
    IQueryable<MatchInfoSource> GetMatchInfoSources();
    IQueryable<Port> GetPorts();
    IQueryable<MatchInfo> GetMatchInfos();
    IQueryable<DpsAttachment> GetDpsAttachments();
    IQueryable<OldHit> GetOldHits();
    IQueryable<User> GetUsers();
    IQueryable<Shipment> GetShipments();
    IQueryable<Client> GetClients();
    IQueryable<AddressType> GetAddressTypes();
    IQueryable<DpsMessage> GetDpsMessages();

    // Mutations
    int AddOrUpdateSettings(int? dbid, Settings settings);
    int AddOrUpdateHitEdit(int? dbid, HitEdit hitEdit);
    int AddOrUpdateCompany(int? dbid, Company company);
    int AddOrUpdateCompanyEntity(int? dbid, CompanyEntity companyEntity);
    int AddOrUpdateSettingsAutoAssignment(int? dbid, SettingsAutoAssignment settingsAutoAssignment);
    int AddOrUpdateWhitelistDuration(int? dbid, WhitelistDuration whitelistDuration);
    int AddOrUpdateHit(int? dbid, Hit hit);
    int AddOrUpdateMatchInfoSource(int? dbid, MatchInfoSource matchInfoSource);
    int AddOrUpdatePort(int? dbid, Port port);
    int AddOrUpdateMatchInfo(int? dbid, MatchInfo matchInfo);
    int AddOrUpdateDpsAttachment(int? dbid, DpsAttachment dpsAttachment);
    int AddOrUpdateOldHit(int? dbid, OldHit oldHit);
    int AddOrUpdateUser(int? dbid, User user);
    int AddOrUpdateShipment(int? dbid, Shipment shipment);
    int AddOrUpdateClient(int? dbid, Client client);
    int AddOrUpdateAddressType(int? dbid, AddressType addressType);
    int AddOrUpdateDpsMessage(int? dbid, DpsMessage dpsMessage);
  }
}
