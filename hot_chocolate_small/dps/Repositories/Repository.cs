using System;
using System.Collections.Generic;
using System.Linq;

using x10.hotchoc.dps.Entities;

namespace x10.hotchoc.dps.Repositories {
  public class Repository : RepositoryBase, IRepository {
    private Dictionary<int, Company> _companies = new Dictionary<int, Company>();
    private Dictionary<int, CompanyEntity> _companyEntities = new Dictionary<int, CompanyEntity>();
    private Dictionary<int, Hit> _hits = new Dictionary<int, Hit>();
    private Dictionary<int, Attachment> _attachments = new Dictionary<int, Attachment>();
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
        typeof(Mutations),
        typeof(Company),
        typeof(CompanyEntity),
        typeof(Hit),
        typeof(Attachment),
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
      int id = instance.Dbid;

      if (instance is Company company) _companies[id] = company;
      if (instance is CompanyEntity companyEntity) _companyEntities[id] = companyEntity;
      if (instance is Hit hit) _hits[id] = hit;
      if (instance is Attachment attachment) _attachments[id] = attachment;
      if (instance is Port port) _ports[id] = port;
      if (instance is MatchInfo matchInfo) _matchInfos[id] = matchInfo;
      if (instance is OldHit oldHit) _oldHits[id] = oldHit;
      if (instance is User user) _users[id] = user;
      if (instance is Shipment shipment) _shipments[id] = shipment;
      if (instance is Client client) _clients[id] = client;
      if (instance is AddressType addressType) _addressTypes[id] = addressType;
      if (instance is Message message) _messages[id] = message;
    }

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
