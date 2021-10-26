using System.Collections.Generic;

using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

using x10.hotchoc.dps.Entities;
using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps {
  [ExtendObjectType(Name = "Query")]
  public partial class Queries {

    #region Settings
    /// <summary>
    /// Retrieve a Settings by id
    /// </summary>
    /// <param name="id">The id of the Settings.</param>
    /// <param name="repository"></param>
    /// <returns>The Settings.</returns>
    public Settings GetSettings(
        string id,
        [Service] IRepository repository) =>
          repository.GetSettings(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Settingses.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Settingses.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Settings> GetSettingses(
        [Service] IRepository repository) =>
          repository.GetSettingses();
    #endregion

    #region HitEdit
    /// <summary>
    /// Retrieve a HitEdit by id
    /// </summary>
    /// <param name="id">The id of the HitEdit.</param>
    /// <param name="repository"></param>
    /// <returns>The HitEdit.</returns>
    public HitEdit GetHitEdit(
        string id,
        [Service] IRepository repository) =>
          repository.GetHitEdit(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all HitEdits.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All HitEdits.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<HitEdit> GetHitEdits(
        [Service] IRepository repository) =>
          repository.GetHitEdits();
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

    #region SettingsAutoAssignment
    /// <summary>
    /// Retrieve a SettingsAutoAssignment by id
    /// </summary>
    /// <param name="id">The id of the SettingsAutoAssignment.</param>
    /// <param name="repository"></param>
    /// <returns>The SettingsAutoAssignment.</returns>
    public SettingsAutoAssignment GetSettingsAutoAssignment(
        string id,
        [Service] IRepository repository) =>
          repository.GetSettingsAutoAssignment(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all SettingsAutoAssignments.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All SettingsAutoAssignments.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<SettingsAutoAssignment> GetSettingsAutoAssignments(
        [Service] IRepository repository) =>
          repository.GetSettingsAutoAssignments();
    #endregion

    #region WhitelistDuration
    /// <summary>
    /// Retrieve a WhitelistDuration by id
    /// </summary>
    /// <param name="id">The id of the WhitelistDuration.</param>
    /// <param name="repository"></param>
    /// <returns>The WhitelistDuration.</returns>
    public WhitelistDuration GetWhitelistDuration(
        string id,
        [Service] IRepository repository) =>
          repository.GetWhitelistDuration(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all WhitelistDurations.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All WhitelistDurations.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<WhitelistDuration> GetWhitelistDurations(
        [Service] IRepository repository) =>
          repository.GetWhitelistDurations();
    #endregion

    #region Hit
    /// <summary>
    /// Retrieve a Hit by id
    /// </summary>
    /// <param name="id">The id of the Hit.</param>
    /// <param name="repository"></param>
    /// <returns>The Hit.</returns>
    public Hit GetHit(
        string id,
        [Service] IRepository repository) =>
          repository.GetHit(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Hits.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Hits.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Hit> GetHits(
        [Service] IRepository repository) =>
          repository.GetHits();
    #endregion

    #region Attachment
    /// <summary>
    /// Retrieve a Attachment by id
    /// </summary>
    /// <param name="id">The id of the Attachment.</param>
    /// <param name="repository"></param>
    /// <returns>The Attachment.</returns>
    public Attachment GetAttachment(
        string id,
        [Service] IRepository repository) =>
          repository.GetAttachment(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Attachments.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Attachments.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Attachment> GetAttachments(
        [Service] IRepository repository) =>
          repository.GetAttachments();
    #endregion

    #region MatchInfoSource
    /// <summary>
    /// Retrieve a MatchInfoSource by id
    /// </summary>
    /// <param name="id">The id of the MatchInfoSource.</param>
    /// <param name="repository"></param>
    /// <returns>The MatchInfoSource.</returns>
    public MatchInfoSource GetMatchInfoSource(
        string id,
        [Service] IRepository repository) =>
          repository.GetMatchInfoSource(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all MatchInfoSources.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All MatchInfoSources.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<MatchInfoSource> GetMatchInfoSources(
        [Service] IRepository repository) =>
          repository.GetMatchInfoSources();
    #endregion

    #region Port
    /// <summary>
    /// Retrieve a Port by id
    /// </summary>
    /// <param name="id">The id of the Port.</param>
    /// <param name="repository"></param>
    /// <returns>The Port.</returns>
    public Port GetPort(
        string id,
        [Service] IRepository repository) =>
          repository.GetPort(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Ports.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Ports.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Port> GetPorts(
        [Service] IRepository repository) =>
          repository.GetPorts();
    #endregion

    #region MatchInfo
    /// <summary>
    /// Retrieve a MatchInfo by id
    /// </summary>
    /// <param name="id">The id of the MatchInfo.</param>
    /// <param name="repository"></param>
    /// <returns>The MatchInfo.</returns>
    public MatchInfo GetMatchInfo(
        string id,
        [Service] IRepository repository) =>
          repository.GetMatchInfo(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all MatchInfos.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All MatchInfos.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<MatchInfo> GetMatchInfos(
        [Service] IRepository repository) =>
          repository.GetMatchInfos();
    #endregion

    #region OldHit
    /// <summary>
    /// Retrieve a OldHit by id
    /// </summary>
    /// <param name="id">The id of the OldHit.</param>
    /// <param name="repository"></param>
    /// <returns>The OldHit.</returns>
    public OldHit GetOldHit(
        string id,
        [Service] IRepository repository) =>
          repository.GetOldHit(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all OldHits.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All OldHits.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<OldHit> GetOldHits(
        [Service] IRepository repository) =>
          repository.GetOldHits();
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

    #region Shipment
    /// <summary>
    /// Retrieve a Shipment by id
    /// </summary>
    /// <param name="id">The id of the Shipment.</param>
    /// <param name="repository"></param>
    /// <returns>The Shipment.</returns>
    public Shipment GetShipment(
        string id,
        [Service] IRepository repository) =>
          repository.GetShipment(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Shipments.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Shipments.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Shipment> GetShipments(
        [Service] IRepository repository) =>
          repository.GetShipments();
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

    #region AddressType
    /// <summary>
    /// Retrieve a AddressType by id
    /// </summary>
    /// <param name="id">The id of the AddressType.</param>
    /// <param name="repository"></param>
    /// <returns>The AddressType.</returns>
    public AddressType GetAddressType(
        string id,
        [Service] IRepository repository) =>
          repository.GetAddressType(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all AddressTypes.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All AddressTypes.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<AddressType> GetAddressTypes(
        [Service] IRepository repository) =>
          repository.GetAddressTypes();
    #endregion

    #region Message
    /// <summary>
    /// Retrieve a Message by id
    /// </summary>
    /// <param name="id">The id of the Message.</param>
    /// <param name="repository"></param>
    /// <returns>The Message.</returns>
    public Message GetMessage(
        string id,
        [Service] IRepository repository) =>
          repository.GetMessage(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Messages.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Messages.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Message> GetMessages(
        [Service] IRepository repository) =>
          repository.GetMessages();
    #endregion

  }
}
