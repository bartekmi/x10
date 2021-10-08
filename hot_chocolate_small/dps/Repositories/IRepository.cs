using System;
using System.Collections.Generic;
using System.Linq;

using x10.hotchoc.dps.Entities;

namespace x10.hotchoc.dps.Repositories {
  public interface IRepository {
    // Queries
    SuggestedResource GetSuggestedResource(int id);
    CompanyEntity GetCompanyEntity(int id);
    Hit GetHit(int id);
    Attachment GetAttachment(int id);
    MatchInfo GetMatchInfo(int id);
    OldHit GetOldHit(int id);
    User GetUser(int id);
    Shipment GetShipment(int id);
    Message GetMessage(int id);

    IQueryable<SuggestedResource> GetSuggestedResources();
    IQueryable<CompanyEntity> GetCompanyEntities();
    IQueryable<Hit> GetHits();
    IQueryable<Attachment> GetAttachments();
    IQueryable<MatchInfo> GetMatchInfos();
    IQueryable<OldHit> GetOldHits();
    IQueryable<User> GetUsers();
    IQueryable<Shipment> GetShipments();
    IQueryable<Message> GetMessages();

    // Mutations
    int AddOrUpdateSuggestedResource(int? dbid, SuggestedResource suggestedResource);
    int AddOrUpdateCompanyEntity(int? dbid, CompanyEntity companyEntity);
    int AddOrUpdateHit(int? dbid, Hit hit);
    int AddOrUpdateAttachment(int? dbid, Attachment attachment);
    int AddOrUpdateMatchInfo(int? dbid, MatchInfo matchInfo);
    int AddOrUpdateOldHit(int? dbid, OldHit oldHit);
    int AddOrUpdateUser(int? dbid, User user);
    int AddOrUpdateShipment(int? dbid, Shipment shipment);
    int AddOrUpdateMessage(int? dbid, Message message);
  }
}
