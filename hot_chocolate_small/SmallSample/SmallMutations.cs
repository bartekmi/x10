using System;

using HotChocolate;
using HotChocolate.Types;

using Small.Entities;
using Small.Repositories;

namespace Small {
  [ExtendObjectType(Name = "Mutation")]
  public class SmallMutations {
    const int BLANK_DBID = -1;

    /// <summary>
    /// Creates a new Building
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
  }
}
