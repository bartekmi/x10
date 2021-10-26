using System;
using System.Collections.Generic;
using System.Linq;

using x10.hotchoc.dps.Entities;

namespace x10.hotchoc.dps.Repositories {
  public class Repository : RepositoryBase, IRepository {
    private Dictionary<int, Settings> _settingses = new Dictionary<int, Settings>();
    private Dictionary<int, HitEdit> _hitEdits = new Dictionary<int, HitEdit>();
    private Dictionary<int, Company> _companies = new Dictionary<int, Company>();
    private Dictionary<int, CompanyEntity> _companyEntities = new Dictionary<int, CompanyEntity>();
    private Dictionary<int, SettingsAutoAssignment> _settingsAutoAssignments = new Dictionary<int, SettingsAutoAssignment>();
    private Dictionary<int, WhitelistDuration> _whitelistDurations = new Dictionary<int, WhitelistDuration>();
    private Dictionary<int, Hit> _hits = new Dictionary<int, Hit>();
    private Dictionary<int, Attachment> _attachments = new Dictionary<int, Attachment>();
    private Dictionary<int, MatchInfoSource> _matchInfoSources = new Dictionary<int, MatchInfoSource>();
    private Dictionary<int, Port> _ports = new Dictionary<int, Port>();
    private Dictionary<int, MatchInfo> _matchInfos = new Dictionary<int, MatchInfo>();
    private Dictionary<int, OldHit> _oldHits = new Dictionary<int, OldHit>();
    private Dictionary<int, User> _users = new Dictionary<int, User>();
    private Dictionary<int, Shipment> _shipments = new Dictionary<int, Shipment>();
    private Dictionary<int, Client> _clients = new Dictionary<int, Client>();
    private Dictionary<int, AddressType> _addressTypes = new Dictionary<int, AddressType>();
    private Dictionary<int, Message> _messages = new Dictionary<int, Message>();

    public override IEnumerable<Type> Types() {
      return new Type[] {
        typeof(Queries),
        typeof(CustomMutations),
        typeof(Settings),
        typeof(HitEdit),
        typeof(Company),
        typeof(CompanyEntity),
        typeof(SettingsAutoAssignment),
        typeof(WhitelistDuration),
        typeof(Hit),
        typeof(Attachment),
        typeof(MatchInfoSource),
        typeof(Port),
        typeof(MatchInfo),
        typeof(OldHit),
        typeof(User),
        typeof(Shipment),
        typeof(Client),
        typeof(AddressType),
        typeof(Message),
      };
    }

    public override void Add(PrimordialEntityBase instance) {
      int id = instance.DbidHotChoc;

      if (instance is Settings settings) _settingses[id] = settings;
      if (instance is HitEdit hitEdit) _hitEdits[id] = hitEdit;
      if (instance is Company company) _companies[id] = company;
      if (instance is CompanyEntity companyEntity) _companyEntities[id] = companyEntity;
      if (instance is SettingsAutoAssignment settingsAutoAssignment) _settingsAutoAssignments[id] = settingsAutoAssignment;
      if (instance is WhitelistDuration whitelistDuration) _whitelistDurations[id] = whitelistDuration;
      if (instance is Hit hit) _hits[id] = hit;
      if (instance is Attachment attachment) _attachments[id] = attachment;
      if (instance is MatchInfoSource matchInfoSource) _matchInfoSources[id] = matchInfoSource;
      if (instance is Port port) _ports[id] = port;
      if (instance is MatchInfo matchInfo) _matchInfos[id] = matchInfo;
      if (instance is OldHit oldHit) _oldHits[id] = oldHit;
      if (instance is User user) _users[id] = user;
      if (instance is Shipment shipment) _shipments[id] = shipment;
      if (instance is Client client) _clients[id] = client;
      if (instance is AddressType addressType) _addressTypes[id] = addressType;
      if (instance is Message message) _messages[id] = message;
    }

    #region Settingses
    public IQueryable<Settings> GetSettingses() => _settingses.Values.AsQueryable();
    public Settings GetSettings(int id) { return _settingses[id]; }
    public int AddOrUpdateSettings(int? dbid, Settings settings) {
      return RepositoryUtils.AddOrUpdate(dbid, settings, _settingses);
    }
    #endregion

    #region HitEdits
    public IQueryable<HitEdit> GetHitEdits() => _hitEdits.Values.AsQueryable();
    public HitEdit GetHitEdit(int id) { return _hitEdits[id]; }
    public int AddOrUpdateHitEdit(int? dbid, HitEdit hitEdit) {
      return RepositoryUtils.AddOrUpdate(dbid, hitEdit, _hitEdits);
    }
    #endregion

    #region Companies
    public IQueryable<Company> GetCompanies() => _companies.Values.AsQueryable();
    public Company GetCompany(int id) { return _companies[id]; }
    public int AddOrUpdateCompany(int? dbid, Company company) {
      return RepositoryUtils.AddOrUpdate(dbid, company, _companies);
    }
    #endregion

    #region CompanyEntities
    public IQueryable<CompanyEntity> GetCompanyEntities() => _companyEntities.Values.AsQueryable();
    public CompanyEntity GetCompanyEntity(int id) { return _companyEntities[id]; }
    public int AddOrUpdateCompanyEntity(int? dbid, CompanyEntity companyEntity) {
      return RepositoryUtils.AddOrUpdate(dbid, companyEntity, _companyEntities);
    }
    #endregion

    #region SettingsAutoAssignments
    public IQueryable<SettingsAutoAssignment> GetSettingsAutoAssignments() => _settingsAutoAssignments.Values.AsQueryable();
    public SettingsAutoAssignment GetSettingsAutoAssignment(int id) { return _settingsAutoAssignments[id]; }
    public int AddOrUpdateSettingsAutoAssignment(int? dbid, SettingsAutoAssignment settingsAutoAssignment) {
      return RepositoryUtils.AddOrUpdate(dbid, settingsAutoAssignment, _settingsAutoAssignments);
    }
    #endregion

    #region WhitelistDurations
    public IQueryable<WhitelistDuration> GetWhitelistDurations() => _whitelistDurations.Values.AsQueryable();
    public WhitelistDuration GetWhitelistDuration(int id) { return _whitelistDurations[id]; }
    public int AddOrUpdateWhitelistDuration(int? dbid, WhitelistDuration whitelistDuration) {
      return RepositoryUtils.AddOrUpdate(dbid, whitelistDuration, _whitelistDurations);
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

    #region MatchInfoSources
    public IQueryable<MatchInfoSource> GetMatchInfoSources() => _matchInfoSources.Values.AsQueryable();
    public MatchInfoSource GetMatchInfoSource(int id) { return _matchInfoSources[id]; }
    public int AddOrUpdateMatchInfoSource(int? dbid, MatchInfoSource matchInfoSource) {
      return RepositoryUtils.AddOrUpdate(dbid, matchInfoSource, _matchInfoSources);
    }
    #endregion

    #region Ports
    public IQueryable<Port> GetPorts() => _ports.Values.AsQueryable();
    public Port GetPort(int id) { return _ports[id]; }
    public int AddOrUpdatePort(int? dbid, Port port) {
      return RepositoryUtils.AddOrUpdate(dbid, port, _ports);
    }
    #endregion

    #region MatchInfos
    public IQueryable<MatchInfo> GetMatchInfos() => _matchInfos.Values.AsQueryable();
    public MatchInfo GetMatchInfo(int id) { return _matchInfos[id]; }
    public int AddOrUpdateMatchInfo(int? dbid, MatchInfo matchInfo) {
      return RepositoryUtils.AddOrUpdate(dbid, matchInfo, _matchInfos);
    }
    #endregion

    #region OldHits
    public IQueryable<OldHit> GetOldHits() => _oldHits.Values.AsQueryable();
    public OldHit GetOldHit(int id) { return _oldHits[id]; }
    public int AddOrUpdateOldHit(int? dbid, OldHit oldHit) {
      return RepositoryUtils.AddOrUpdate(dbid, oldHit, _oldHits);
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

    #region Clients
    public IQueryable<Client> GetClients() => _clients.Values.AsQueryable();
    public Client GetClient(int id) { return _clients[id]; }
    public int AddOrUpdateClient(int? dbid, Client client) {
      return RepositoryUtils.AddOrUpdate(dbid, client, _clients);
    }
    #endregion

    #region AddressTypes
    public IQueryable<AddressType> GetAddressTypes() => _addressTypes.Values.AsQueryable();
    public AddressType GetAddressType(int id) { return _addressTypes[id]; }
    public int AddOrUpdateAddressType(int? dbid, AddressType addressType) {
      return RepositoryUtils.AddOrUpdate(dbid, addressType, _addressTypes);
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
