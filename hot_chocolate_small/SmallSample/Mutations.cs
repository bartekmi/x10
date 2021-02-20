using System;
using System.Collections.Generic;
using System.Linq;

using HotChocolate;
using HotChocolate.Types;

using x10.hotchoc.SmallSample.Entities;
using x10.hotchoc.SmallSample.Repositories;

namespace x10.hotchoc.SmallSample {
  [ExtendObjectType(Name = "Mutation")]
  public class Mutations {

    #region Building
    /// <summary>
    /// Creates a new Building or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateBuilding(
        string id,
        string name,
        string? description,
        DateTime dateOfOccupancy,
        MailboxTypeEnum mailboxType,
        PetPolicyEnum? petPolicy,
        bool mailingAddressSameAsPhysical,
        IEnumerable<Unit> units,
        Address physicalAddress,
        Address? mailingAddress,
        [Service] IRepository repository) {

      Building building = new Building() {
        Name = name,
        Description = description,
        DateOfOccupancy = dateOfOccupancy,
        MailboxType = mailboxType,
        PetPolicy = petPolicy,
        MailingAddressSameAsPhysical = mailingAddressSameAsPhysical,
        Units = units.ToList(),
        PhysicalAddress = physicalAddress,
        MailingAddress = mailingAddress,
      };

      int dbid = repository.AddOrUpdateBuilding(IdUtils.FromRelayId(id), building);
      return IdUtils.ToRelayId<Building>(dbid);
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
        [Service] IRepository repository) {

      Move move = new Move() {
        Date = date,
        From = repository.GetBuilding(IdUtils.FromRelayIdMandatory(fromId)),
        To = repository.GetBuilding(IdUtils.FromRelayIdMandatory(toId)),
        Tenant = repository.GetTenant(IdUtils.FromRelayIdMandatory(tenantId)),
      };

      int dbid = repository.AddOrUpdateMove(IdUtils.FromRelayId(id), move);
      return IdUtils.ToRelayId<Move>(dbid);
    }
    #endregion

    #region Tenant
    /// <summary>
    /// Creates a new Tenant or updates an existing one, depending on the value of tenant.id
    /// </summary>
    public string CreateOrUpdateTenant(
        Tenant tenant,
        [Service] IRepository repository) {

      int dbid = repository.AddOrUpdateTenant(IdUtils.FromRelayId(tenant.Id), tenant);
      return IdUtils.ToRelayId<Tenant>(dbid);
    }
    #endregion

    #region Address
    /// <summary>
    /// Creates a new Address or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateAddress(
        string id,
        string? unitNumber,
        string theAddress,
        string city,
        string stateOrProvince,
        string zip,
        string countryId,
        [Service] IRepository repository) {

      Address address = new Address() {
        UnitNumber = unitNumber,
        TheAddress = theAddress,
        City = city,
        StateOrProvince = stateOrProvince,
        Zip = zip,
        Country = repository.GetCountry(IdUtils.FromRelayIdMandatory(countryId)),
      };

      int dbid = repository.AddOrUpdateAddress(IdUtils.FromRelayId(id), address);
      return IdUtils.ToRelayId<Address>(dbid);
    }
    #endregion

    #region Country
    /// <summary>
    /// Creates a new Country or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateCountry(
        string id,
        string code,
        string name,
        [Service] IRepository repository) {

      Country country = new Country() {
        Code = code,
        Name = name,
      };

      int dbid = repository.AddOrUpdateCountry(IdUtils.FromRelayId(id), country);
      return IdUtils.ToRelayId<Country>(dbid);
    }
    #endregion

    #region Unit
    /// <summary>
    /// Creates a new Unit or updates an existing one, depending on the value of id
    /// </summary>
    public string CreateOrUpdateUnit(
        string id,
        string number,
        double? squareFeet,
        int numberOfBedrooms,
        NumberOfBathroomsEnum numberOfBathrooms,
        bool hasBalcony,
        [Service] IRepository repository) {

      Unit unit = new Unit() {
        Number = number,
        SquareFeet = squareFeet,
        NumberOfBedrooms = numberOfBedrooms,
        NumberOfBathrooms = numberOfBathrooms,
        HasBalcony = hasBalcony,
      };

      int dbid = repository.AddOrUpdateUnit(IdUtils.FromRelayId(id), unit);
      return IdUtils.ToRelayId<Unit>(dbid);
    }
    #endregion

  }
}
