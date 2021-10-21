using System;
using System.Collections.Generic;
using System.Linq;

using HotChocolate;
using HotChocolate.Types;

using x10.hotchoc.dps.Entities;
using x10.hotchoc.dps.Repositories;

namespace x10.hotchoc.dps {
  [ExtendObjectType(Name = "Mutation")]
  public partial class Mutations {

    #region Company
    /// <summary>
    /// Creates a new Company or updates an existing one, depending on the value of company.id
    /// </summary>
    public string CreateOrUpdateCompany(
      Company company,
      [Service] IRepository repository) {
        company.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateCompany(IdUtils.FromRelayId(company.Id), company);
        return IdUtils.ToRelayId<Company>(dbid);
    }
    #endregion

    #region CompanyEntity
    /// <summary>
    /// Creates a new CompanyEntity or updates an existing one, depending on the value of companyEntity.id
    /// </summary>
    public string CreateOrUpdateCompanyEntity(
      CompanyEntity companyEntity,
      [Service] IRepository repository) {
        companyEntity.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateCompanyEntity(IdUtils.FromRelayId(companyEntity.Id), companyEntity);
        return IdUtils.ToRelayId<CompanyEntity>(dbid);
    }
    #endregion

    #region WhitelistDuration
    /// <summary>
    /// Creates a new WhitelistDuration or updates an existing one, depending on the value of whitelistDuration.id
    /// </summary>
    public string CreateOrUpdateWhitelistDuration(
      WhitelistDuration whitelistDuration,
      [Service] IRepository repository) {
        whitelistDuration.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateWhitelistDuration(IdUtils.FromRelayId(whitelistDuration.Id), whitelistDuration);
        return IdUtils.ToRelayId<WhitelistDuration>(dbid);
    }
    #endregion

    #region Hit
    /// <summary>
    /// Creates a new Hit or updates an existing one, depending on the value of hit.id
    /// </summary>
    public string CreateOrUpdateHit(
      Hit hit,
      [Service] IRepository repository) {
        hit.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateHit(IdUtils.FromRelayId(hit.Id), hit);
        return IdUtils.ToRelayId<Hit>(dbid);
    }
    #endregion

    #region Attachment
    /// <summary>
    /// Creates a new Attachment or updates an existing one, depending on the value of attachment.id
    /// </summary>
    public string CreateOrUpdateAttachment(
      Attachment attachment,
      [Service] IRepository repository) {
        attachment.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateAttachment(IdUtils.FromRelayId(attachment.Id), attachment);
        return IdUtils.ToRelayId<Attachment>(dbid);
    }
    #endregion

    #region MatchInfoSource
    /// <summary>
    /// Creates a new MatchInfoSource or updates an existing one, depending on the value of matchInfoSource.id
    /// </summary>
    public string CreateOrUpdateMatchInfoSource(
      MatchInfoSource matchInfoSource,
      [Service] IRepository repository) {
        matchInfoSource.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateMatchInfoSource(IdUtils.FromRelayId(matchInfoSource.Id), matchInfoSource);
        return IdUtils.ToRelayId<MatchInfoSource>(dbid);
    }
    #endregion

    #region Port
    /// <summary>
    /// Creates a new Port or updates an existing one, depending on the value of port.id
    /// </summary>
    public string CreateOrUpdatePort(
      Port port,
      [Service] IRepository repository) {
        port.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdatePort(IdUtils.FromRelayId(port.Id), port);
        return IdUtils.ToRelayId<Port>(dbid);
    }
    #endregion

    #region MatchInfo
    /// <summary>
    /// Creates a new MatchInfo or updates an existing one, depending on the value of matchInfo.id
    /// </summary>
    public string CreateOrUpdateMatchInfo(
      MatchInfo matchInfo,
      [Service] IRepository repository) {
        matchInfo.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateMatchInfo(IdUtils.FromRelayId(matchInfo.Id), matchInfo);
        return IdUtils.ToRelayId<MatchInfo>(dbid);
    }
    #endregion

    #region OldHit
    /// <summary>
    /// Creates a new OldHit or updates an existing one, depending on the value of oldHit.id
    /// </summary>
    public string CreateOrUpdateOldHit(
      OldHit oldHit,
      [Service] IRepository repository) {
        oldHit.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateOldHit(IdUtils.FromRelayId(oldHit.Id), oldHit);
        return IdUtils.ToRelayId<OldHit>(dbid);
    }
    #endregion

    #region User
    /// <summary>
    /// Creates a new User or updates an existing one, depending on the value of user.id
    /// </summary>
    public string CreateOrUpdateUser(
      User user,
      [Service] IRepository repository) {
        user.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateUser(IdUtils.FromRelayId(user.Id), user);
        return IdUtils.ToRelayId<User>(dbid);
    }
    #endregion

    #region Shipment
    /// <summary>
    /// Creates a new Shipment or updates an existing one, depending on the value of shipment.id
    /// </summary>
    public string CreateOrUpdateShipment(
      Shipment shipment,
      [Service] IRepository repository) {
        shipment.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateShipment(IdUtils.FromRelayId(shipment.Id), shipment);
        return IdUtils.ToRelayId<Shipment>(dbid);
    }
    #endregion

    #region Client
    /// <summary>
    /// Creates a new Client or updates an existing one, depending on the value of client.id
    /// </summary>
    public string CreateOrUpdateClient(
      Client client,
      [Service] IRepository repository) {
        client.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateClient(IdUtils.FromRelayId(client.Id), client);
        return IdUtils.ToRelayId<Client>(dbid);
    }
    #endregion

    #region AddressType
    /// <summary>
    /// Creates a new AddressType or updates an existing one, depending on the value of addressType.id
    /// </summary>
    public string CreateOrUpdateAddressType(
      AddressType addressType,
      [Service] IRepository repository) {
        addressType.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateAddressType(IdUtils.FromRelayId(addressType.Id), addressType);
        return IdUtils.ToRelayId<AddressType>(dbid);
    }
    #endregion

    #region Message
    /// <summary>
    /// Creates a new Message or updates an existing one, depending on the value of message.id
    /// </summary>
    public string CreateOrUpdateMessage(
      Message message,
      [Service] IRepository repository) {
        message.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateMessage(IdUtils.FromRelayId(message.Id), message);
        return IdUtils.ToRelayId<Message>(dbid);
    }
    #endregion

  }
}
