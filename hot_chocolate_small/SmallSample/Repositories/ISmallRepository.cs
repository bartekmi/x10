using System.Collections.Generic;
using System.Linq;

using x10.hotchoc.Entities;

namespace x10.hotchoc.Repositories {
  public interface ISmallRepository {
    // Queries
    Building GetBuilding(int id);
    Tenant GetTenant(int id);
    Move GetMove(int id);

    IQueryable<Building> GetBuildings();
    IQueryable<Tenant> GetTenants();
    IQueryable<Move> GetMoves();

    // Mutations
    int AddBuilding(Building building);
    void UpdateBuilding(Building building);
    int AddTenant(Tenant tenant);
    void UpdateTenant(Tenant tenant);
    int AddMove(Move move);
    void UpdateMove(Move move);
  }
}
