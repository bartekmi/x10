using System;
using System.Collections.Generic;
using System.Linq;

using x10.hotchoc.dps.Entities;

namespace x10.hotchoc.dps.Repositories {
  public class Repository : RepositoryBase, IRepository {
    private Dictionary<int, SuggestedResource> _suggestedResources = new Dictionary<int, SuggestedResource>();
    private Dictionary<int, CompanyEntity> _companyEntities = new Dictionary<int, CompanyEntity>();
    private Dictionary<int, Hit> _hits = new Dictionary<int, Hit>();
    private Dictionary<int, Attachment> _attachments = new Dictionary<int, Attachment>();
    private Dictionary<int, MatchInfo> _matchInfos = new Dictionary<int, MatchInfo>();
    private Dictionary<int, User> _users = new Dictionary<int, User>();
    private Dictionary<int, Shipment> _shipments = new Dictionary<int, Shipment>();
    private Dictionary<int, Message> _messages = new Dictionary<int, Message>();

    public override IEnumerable<Type> Types() {
      return new Type[] {
        typeof(Queries),
        typeof(Mutations),
        typeof(SuggestedResource),
        typeof(CompanyEntity),
        typeof(Hit),
        typeof(Attachment),
        typeof(MatchInfo),
        typeof(User),
        typeof(Shipment),
        typeof(Message),
      };
    }

    public override void Add(PrimordialEntityBase instance) {
      int id = instance.Dbid;

      if (instance is SuggestedResource suggestedResource) _suggestedResources[id] = suggestedResource;
      if (instance is CompanyEntity companyEntity) _companyEntities[id] = companyEntity;
      if (instance is Hit hit) _hits[id] = hit;
      if (instance is Attachment attachment) _attachments[id] = attachment;
      if (instance is MatchInfo matchInfo) _matchInfos[id] = matchInfo;
      if (instance is User user) _users[id] = user;
      if (instance is Shipment shipment) _shipments[id] = shipment;
      if (instance is Message message) _messages[id] = message;
    }

    #region SuggestedResources
    public IQueryable<SuggestedResource> GetSuggestedResources() => _suggestedResources.Values.AsQueryable();
    public SuggestedResource GetSuggestedResource(int id) { return _suggestedResources[id]; }
    public int AddOrUpdateSuggestedResource(int? dbid, SuggestedResource suggestedResource) {
      return RepositoryUtils.AddOrUpdate(dbid, suggestedResource, _suggestedResources);
    }
    #endregion

    #region CompanyEntities
    public IQueryable<CompanyEntity> GetCompanyEntities() => _companyEntities.Values.AsQueryable();
    public CompanyEntity GetCompanyEntity(int id) { return _companyEntities[id]; }
    public int AddOrUpdateCompanyEntity(int? dbid, CompanyEntity companyEntity) {
      return RepositoryUtils.AddOrUpdate(dbid, companyEntity, _companyEntities);
    }
    #endregion

    #region Hits
    public IQueryable<Hit> GetHits() => _hits.Values.AsQueryable();
    public Hit GetHit(int id) { return _hits[id]; }
    public int AddOrUpdateHit(int? dbid, Hit hit) {
      return RepositoryUtils.AddOrUpdate(dbid, hit, _hits);
    }
    #endregion

    #region Attachments
    public IQueryable<Attachment> GetAttachments() => _attachments.Values.AsQueryable();
    public Attachment GetAttachment(int id) { return _attachments[id]; }
    public int AddOrUpdateAttachment(int? dbid, Attachment attachment) {
      return RepositoryUtils.AddOrUpdate(dbid, attachment, _attachments);
    }
    #endregion

    #region MatchInfos
    public IQueryable<MatchInfo> GetMatchInfos() => _matchInfos.Values.AsQueryable();
    public MatchInfo GetMatchInfo(int id) { return _matchInfos[id]; }
    public int AddOrUpdateMatchInfo(int? dbid, MatchInfo matchInfo) {
      return RepositoryUtils.AddOrUpdate(dbid, matchInfo, _matchInfos);
    }
    #endregion

    #region Users
    public IQueryable<User> GetUsers() => _users.Values.AsQueryable();
    public User GetUser(int id) { return _users[id]; }
    public int AddOrUpdateUser(int? dbid, User user) {
      return RepositoryUtils.AddOrUpdate(dbid, user, _users);
    }
    #endregion

    #region Shipments
    public IQueryable<Shipment> GetShipments() => _shipments.Values.AsQueryable();
    public Shipment GetShipment(int id) { return _shipments[id]; }
    public int AddOrUpdateShipment(int? dbid, Shipment shipment) {
      return RepositoryUtils.AddOrUpdate(dbid, shipment, _shipments);
    }
    #endregion

    #region Messages
    public IQueryable<Message> GetMessages() => _messages.Values.AsQueryable();
    public Message GetMessage(int id) { return _messages[id]; }
    public int AddOrUpdateMessage(int? dbid, Message message) {
      return RepositoryUtils.AddOrUpdate(dbid, message, _messages);
    }
    #endregion

  }
}
