using System.Collections.Generic;

using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

using x10.hotchoc.Entities;
using x10.hotchoc.Repositories;

namespace x10.hotchoc {
  [ExtendObjectType(Name = "Query")]
  public class SmallQueries {

    # region Building

    /// <summary>
    /// Retrieve a Building by id
    /// </summary>
    /// <param name="id">The id of the building.</param>
    /// <param name="repository"></param>
    /// <returns>The Building.</returns>
    public Building GetBuilding(
        string id,
        [Service] ISmallRepository repository) =>
        repository.GetBuilding(IdUtils.FromRelayId(id));

    /// <summary>
    /// Gets all buildings.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Buildings.</returns>
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Building> GetBuildings(
        [Service] ISmallRepository repository) =>
        repository.GetBuildings();
    #endregion

    #region Tenant
    /// <summary>
    /// Retrieve a Tenant by id
    /// </summary>
    /// <param name="id">The id of the tenant.</param>
    /// <param name="repository"></param>
    /// <returns>The Tenant.</returns>
    public Tenant GetTenant(
        string id,
        [Service] ISmallRepository repository) =>
        repository.GetTenant(IdUtils.FromRelayId(id));

    /// <summary>
    /// Gets all tenants.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Tenants.</returns>
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Tenant> GetTenants(
        [Service] ISmallRepository repository) =>
        repository.GetTenants();
    #endregion

    #region Move
    /// <summary>
    /// Retrieve a Move by id
    /// </summary>
    /// <param name="id">The id of the move.</param>
    /// <param name="repository"></param>
    /// <returns>The Move.</returns>
    public Move GetMove(
        string id,
        [Service] ISmallRepository repository) =>
        repository.GetMove(IdUtils.FromRelayId(id));

    /// <summary>
    /// Gets all moves.
    /// </summary>
    /// <param name="repository"></param>
    /// <returns>All Moves.</returns>
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public IEnumerable<Move> GetMoves(
        [Service] ISmallRepository repository) =>
        repository.GetMoves();
    #endregion
  }
}
