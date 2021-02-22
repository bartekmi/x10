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
    /// Creates a new Building or updates an existing one, depending on the value of building.id
    /// </summary>
    public string CreateOrUpdateBuilding(
      Building building,
      [Service] IRepository repository) {
        building.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateBuilding(IdUtils.FromRelayId(building.Id), building);
        return IdUtils.ToRelayId<Building>(dbid);
    }
    #endregion

    #region Move
    /// <summary>
    /// Creates a new Move or updates an existing one, depending on the value of move.id
    /// </summary>
    public string CreateOrUpdateMove(
      Move move,
      [Service] IRepository repository) {
        move.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateMove(IdUtils.FromRelayId(move.Id), move);
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
        tenant.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateTenant(IdUtils.FromRelayId(tenant.Id), tenant);
        return IdUtils.ToRelayId<Tenant>(dbid);
    }
    #endregion

    #region Address
    /// <summary>
    /// Creates a new Address or updates an existing one, depending on the value of address.id
    /// </summary>
    public string CreateOrUpdateAddress(
      Address address,
      [Service] IRepository repository) {
        address.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateAddress(IdUtils.FromRelayId(address.Id), address);
        return IdUtils.ToRelayId<Address>(dbid);
    }
    #endregion

    #region Country
    /// <summary>
    /// Creates a new Country or updates an existing one, depending on the value of country.id
    /// </summary>
    public string CreateOrUpdateCountry(
      Country country,
      [Service] IRepository repository) {
        country.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateCountry(IdUtils.FromRelayId(country.Id), country);
        return IdUtils.ToRelayId<Country>(dbid);
    }
    #endregion

    #region Unit
    /// <summary>
    /// Creates a new Unit or updates an existing one, depending on the value of unit.id
    /// </summary>
    public string CreateOrUpdateUnit(
      Unit unit,
      [Service] IRepository repository) {
        unit.SetNonOwnedAssociations(repository);
        int dbid = repository.AddOrUpdateUnit(IdUtils.FromRelayId(unit.Id), unit);
        return IdUtils.ToRelayId<Unit>(dbid);
    }
    #endregion

  }
}
