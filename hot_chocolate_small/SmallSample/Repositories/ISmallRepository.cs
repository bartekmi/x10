using System.Collections.Generic;
using System.Linq;

using Small.Entities;

namespace Small.Repositories {
  public interface ISmallRepository {
    // Queries
    Building GetBuilding(int id);
    Tenant GetTenant(int id);

    IQueryable<Building> GetBuildings();
    IQueryable<Tenant> GetTenants();
    IQueryable<Move> GetMoves();

    // Mutations
    int AddBuilding(Building building);
    void UpdateBuilding(Building building);
  }
}
