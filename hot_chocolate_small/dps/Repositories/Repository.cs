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
    private Dictionary<int, MatchInfoSource> _matchInfoSources = new Dictionary<int, MatchInfoSource>();
    private Dictionary<int, Port> _ports = new Dictionary<int, Port>();
    private Dictionary<int, MatchInfo> _matchInfos = new Dictionary<int, MatchInfo>();
    private Dictionary<int, Booking> _bookings = new Dictionary<int, Booking>();
    private Dictionary<int, DpsAttachment> _dpsAttachments = new Dictionary<int, DpsAttachment>();
    private Dictionary<int, OldHit> _oldHits = new Dictionary<int, OldHit>();
    private Dictionary<int, User> _users = new Dictionary<int, User>();
    private Dictionary<int, Quote> _quotes = new Dictionary<int, Quote>();
    private Dictionary<int, Cargo> _cargos = new Dictionary<int, Cargo>();
    private Dictionary<int, Shipment> _shipments = new Dictionary<int, Shipment>();
    private Dictionary<int, Client> _clients = new Dictionary<int, Client>();
    private Dictionary<int, AddressType> _addressTypes = new Dictionary<int, AddressType>();
    private Dictionary<int, DpsMessage> _dpsMessages = new Dictionary<int, DpsMessage>();

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
        typeof(MatchInfoSource),
        typeof(Port),
        typeof(MatchInfo),
        typeof(Booking),
        typeof(DpsAttachment),
        typeof(OldHit),
        typeof(User),
        typeof(Quote),
        typeof(Cargo),
        typeof(Shipment),
        typeof(Client),
        typeof(AddressType),
        typeof(DpsMessage),
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
      if (instance is MatchInfoSource matchInfoSource) _matchInfoSources[id] = matchInfoSource;
      if (instance is Port port) _ports[id] = port;
      if (instance is MatchInfo matchInfo) _matchInfos[id] = matchInfo;
      if (instance is Booking booking) _bookings[id] = booking;
      if (instance is DpsAttachment dpsAttachment) _dpsAttachments[id] = dpsAttachment;
      if (instance is OldHit oldHit) _oldHits[id] = oldHit;
      if (instance is User user) _users[id] = user;
      if (instance is Quote quote) _quotes[id] = quote;
      if (instance is Cargo cargo) _cargos[id] = cargo;
      if (instance is Shipment shipment) _shipments[id] = shipment;
      if (instance is Client client) _clients[id] = client;
      if (instance is AddressType addressType) _addressTypes[id] = addressType;
      if (instance is DpsMessage dpsMessage) _dpsMessages[id] = dpsMessage;
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

    #region Bookings
    public IQueryable<Booking> GetBookings() => _bookings.Values.AsQueryable();
    public Booking GetBooking(int id) { return _bookings[id]; }
    public int AddOrUpdateBooking(int? dbid, Booking booking) {
      return RepositoryUtils.AddOrUpdate(dbid, booking, _bookings);
    }
    #endregion

    #region DpsAttachments
    public IQueryable<DpsAttachment> GetDpsAttachments() => _dpsAttachments.Values.AsQueryable();
    public DpsAttachment GetDpsAttachment(int id) { return _dpsAttachments[id]; }
    public int AddOrUpdateDpsAttachment(int? dbid, DpsAttachment dpsAttachment) {
      return RepositoryUtils.AddOrUpdate(dbid, dpsAttachment, _dpsAttachments);
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

    #region Quotes
    public IQueryable<Quote> GetQuotes() => _quotes.Values.AsQueryable();
    public Quote GetQuote(int id) { return _quotes[id]; }
    public int AddOrUpdateQuote(int? dbid, Quote quote) {
      return RepositoryUtils.AddOrUpdate(dbid, quote, _quotes);
    }
    #endregion

    #region Cargos
    public IQueryable<Cargo> GetCargos() => _cargos.Values.AsQueryable();
    public Cargo GetCargo(int id) { return _cargos[id]; }
    public int AddOrUpdateCargo(int? dbid, Cargo cargo) {
      return RepositoryUtils.AddOrUpdate(dbid, cargo, _cargos);
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

    #region DpsMessages
    public IQueryable<DpsMessage> GetDpsMessages() => _dpsMessages.Values.AsQueryable();
    public DpsMessage GetDpsMessage(int id) { return _dpsMessages[id]; }
    public int AddOrUpdateDpsMessage(int? dbid, DpsMessage dpsMessage) {
      return RepositoryUtils.AddOrUpdate(dbid, dpsMessage, _dpsMessages);
    }
    #endregion

  }
}
