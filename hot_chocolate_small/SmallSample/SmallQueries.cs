using System.Collections.Generic;

using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

using Small.Entities;
using Small.Repositories;

namespace Small {
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
        int id,
        [Service] ISmallRepository repository) =>
        repository.GetBuilding(id);

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
        int id,
        [Service] ISmallRepository repository) =>
        repository.GetTenant(id);

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
  }
}
