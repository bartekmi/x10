using System.Collections.Generic;

using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

using x10.hotchoc.Entities;
using x10.hotchoc.Repositories;

namespace x10.hotchoc {
  [ExtendObjectType(Name = "Query")]
  public class Queries {

    #region Building
    /// <summary>
    /// Retrieve a Building by id
    /// </summary>
    /// <param name="id">The id of the Building.</param>
    /// <param name="repository"></param>
    /// <returns>The Building.</returns>
    public Building GetBuilding(
        string id,
        [Service] IRepository repository) =>
          repository.GetBuilding(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Buildings.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Buildings.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Building> GetBuildings(
        [Service] IRepository repository) =>
          repository.GetBuildings();
    #endregion

    #region Move
    /// <summary>
    /// Retrieve a Move by id
    /// </summary>
    /// <param name="id">The id of the Move.</param>
    /// <param name="repository"></param>
    /// <returns>The Move.</returns>
    public Move GetMove(
        string id,
        [Service] IRepository repository) =>
          repository.GetMove(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Moves.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Moves.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Move> GetMoves(
        [Service] IRepository repository) =>
          repository.GetMoves();
    #endregion

    #region Tenant
    /// <summary>
    /// Retrieve a Tenant by id
    /// </summary>
    /// <param name="id">The id of the Tenant.</param>
    /// <param name="repository"></param>
    /// <returns>The Tenant.</returns>
    public Tenant GetTenant(
        string id,
        [Service] IRepository repository) =>
          repository.GetTenant(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Tenants.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Tenants.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Tenant> GetTenants(
        [Service] IRepository repository) =>
          repository.GetTenants();
    #endregion

    #region Address
    /// <summary>
    /// Retrieve a Address by id
    /// </summary>
    /// <param name="id">The id of the Address.</param>
    /// <param name="repository"></param>
    /// <returns>The Address.</returns>
    public Address GetAddress(
        string id,
        [Service] IRepository repository) =>
          repository.GetAddress(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Addresses.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Addresses.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Address> GetAddresses(
        [Service] IRepository repository) =>
          repository.GetAddresses();
    #endregion

    #region Unit
    /// <summary>
    /// Retrieve a Unit by id
    /// </summary>
    /// <param name="id">The id of the Unit.</param>
    /// <param name="repository"></param>
    /// <returns>The Unit.</returns>
    public Unit GetUnit(
        string id,
        [Service] IRepository repository) =>
          repository.GetUnit(IdUtils.FromRelayIdMandatory(id));

    /// <summary>
    /// Gets all Units.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Units.</returns>
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Unit> GetUnits(
        [Service] IRepository repository) =>
          repository.GetUnits();
    #endregion

  }
}
