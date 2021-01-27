using System.Collections.Generic;
using System.Linq;

using x10.hotchoc.Entities;

namespace x10.hotchoc.Repositories {
  public interface IRepository {
    // Queries
    Building GetBuilding(int id);
    Move GetMove(int id);
    Tenant GetTenant(int id);
    Address GetAddress(int id);
    Unit GetUnit(int id);

    IQueryable<Building> GetBuildings();
    IQueryable<Move> GetMoves();
    IQueryable<Tenant> GetTenants();
    IQueryable<Address> GetAddresses();
    IQueryable<Unit> GetUnits();

    // Mutations
    int AddOrUpdateBuilding(int? dbid, Building building);

    int AddMove(Move move);
    void UpdateMove(Move move);
    int AddTenant(Tenant tenant);
    void UpdateTenant(Tenant tenant);
    int AddAddress(Address address);
    void UpdateAddress(Address address);
    int AddUnit(Unit unit);
    void UpdateUnit(Unit unit);
  }
}
