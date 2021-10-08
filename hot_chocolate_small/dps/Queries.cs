using System.Collections.Generic;

using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

using x10.hotchoc.dps.Entities;
using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps {
  [ExtendObjectType(Name = "Query")]
  public partial class Queries {

    #region SuggestedResource
    /// <summary>
    /// Retrieve a SuggestedResource by id
    /// </summary>
    /// <param name="id">The id of the SuggestedResource.</param>
    /// <param name="repository"></param>
    /// <returns>The SuggestedResource.</returns>
    public SuggestedResource GetSuggestedResource(
        string id,
        [Service] IRepository repository) =>
          repository.GetSuggestedResource(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all SuggestedResources.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All SuggestedResources.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<SuggestedResource> GetSuggestedResources(
        [Service] IRepository repository) =>
          repository.GetSuggestedResources();
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