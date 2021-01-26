using System;
using System.Collections.Generic;

using HotChocolate;
using HotChocolate.Types;

using x10.hotchoc.Entities;
using x10.hotchoc.Repositories;

namespace x10.hotchoc {
  [ExtendObjectType(Name = "Mutation")]
  public class SmallMutations {
    const int BLANK_DBID = -1;

    #region Building
    /// <summary>
    /// Creates a new Building or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateBuilding(
        string id,
        string name,
        string? description,
        DateTime dateOfOccupancy,
        MailboxTypeEnum? mailboxType,
        PetPolicyEnum? petPolicy,
        bool mailingAddressSameAsPhysical,
        Address physicalAddress,
        Address? mailingAddress,
        IEnumerable<Unit> units,
        [Service] ISmallRepository repository) {

      Building building = new Building() {
        Name = name,
        Description = description,
        DateOfOccupancy = dateOfOccupancy,
        MailboxType = mailboxType,
        PetPolicy = petPolicy,
        MailingAddressSameAsPhysical = mailingAddressSameAsPhysical,
        PhysicalAddress = physicalAddress,
        MailingAddress = mailingAddress,
        Units = new List<Unit>(units),
      };

      if (IdUtils.IsUuid(id)) {
        repository.AddBuilding(building);
        id = building.Id;
      } else {
        building.Dbid = IdUtils.FromRelayId(id);
        repository.UpdateBuilding(building);
      }

      return id;
    }
    #endregion

    #region Tenant
    /// <summary>
    /// Creates a new Tenant or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateTenant(
        string id,
        string name,
        string phone,
        string email,
        Address permanentMailingAddress,
        [Service] ISmallRepository repository) {

      Tenant tenant = new Tenant() {
        Name = name,
        Phone = phone,
        Email = email,
        PermanentMailingAddress = permanentMailingAddress,
      };

      if (IdUtils.IsUuid(id)) {
        repository.AddTenant(tenant);
        id = tenant.Id;
      } else {
        tenant.Dbid = IdUtils.FromRelayId(id);
        repository.UpdateTenant(tenant);
      }

      return id;
    }
    #endregion

    #region Move
    /// <summary>
    /// Creates a new Move or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateMove(
        string id,
        DateTime date,
        string fromId,
        string toId,
        string tenantId,
        [Service] ISmallRepository repository) {

      Move move = new Move() {
        Date = date,
        From = repository.GetBuilding(IdUtils.FromRelayId(fromId)),
        To = repository.GetBuilding(IdUtils.FromRelayId(toId)),
        Tenant = repository.GetTenant(IdUtils.FromRelayId(tenantId)),
      };

      if (IdUtils.IsUuid(id)) {
        repository.AddMove(move);
        id = move.Id;
      } else {
        move.Dbid = IdUtils.FromRelayId(id);
        repository.UpdateMove(move);
      }

      return id;
    }
    #endregion

  }
}
