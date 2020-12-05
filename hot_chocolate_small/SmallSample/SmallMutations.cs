using System;

using HotChocolate;
using HotChocolate.Types;

using Small.Entities;
using Small.Repositories;

namespace Small {
  [ExtendObjectType(Name = "Mutation")]
  public class SmallMutations {
    const int BLANK_DBID = -1;

    #region Building
    /// <summary>
    /// Creates a new Building or updates an existing one, depending on the value of dbid
    /// </summary>
    public int CreateOrUpdateBuilding(
        int dbid,
        string name,
        string? description,
        DateTime dateOfOccupancy,
        MailboxTypeEnum? mailboxType,
        PetPolicyEnum? petPolicy,
        bool mailingAddressSameAsPhysical,
        Address physicalAddress,
        Address? mailingAddress,
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
      };

      if (dbid == BLANK_DBID)
        dbid = repository.AddBuilding(building);
      else {
        building.Dbid = dbid;
        repository.UpdateBuilding(building);
      }

      return dbid;
    }
    #endregion

    #region Building
    /// <summary>
    /// Creates a new Tenant or updates an existing one, depending on the value of dbid
    /// </summary>
    public int CreateOrUpdateTenant(
        int dbid,
        string name,
        string phone,
        string email,
        [Service] ISmallRepository repository) {

      Tenant tenant = new Tenant() {
        Name = name,
        Phone = phone,
        Email = email,
      };

      if (dbid == BLANK_DBID)
        dbid = repository.AddTenant(tenant);
      else {
        tenant.Dbid = dbid;
        repository.UpdateTenant(tenant);
      }

      return dbid;
    }
    #endregion
  }
}
