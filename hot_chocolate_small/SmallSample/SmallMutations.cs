using System;

using HotChocolate;
using HotChocolate.Types;

using Small.Entities;
using Small.Repositories;

namespace Small {
  [ExtendObjectType(Name = "Mutation")]
  public class SmallMutations {
    /// <summary>
    /// Creates a new Building
    /// </summary>
    public int CreateOrUpdateBuilding(
        int? dbid,  // Presence of dbid indicate an update vs an insert
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

      if (dbid == null)
        dbid = repository.AddBuilding(building);
      else 
        repository.UpdateBuilding(building);

      return dbid.Value;
    }
  }
}
