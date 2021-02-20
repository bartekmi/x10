using System;
using System.Collections.Generic;
using System.Linq;

using x10.hotchoc.SmallSample.Entities;

namespace x10.hotchoc.SmallSample.Repositories {
  public interface IRepository {
    // Queries
    Building GetBuilding(int id);
    Move GetMove(int id);
    Tenant GetTenant(int id);
    Address GetAddress(int id);
    Country GetCountry(int id);
    Unit GetUnit(int id);

    IQueryable<Building> GetBuildings();
    IQueryable<Move> GetMoves();
    IQueryable<Tenant> GetTenants();
    IQueryable<Address> GetAddresses();
    IQueryable<Country> GetCountries();
    IQueryable<Unit> GetUnits();

    // Mutations
    int AddOrUpdateBuilding(int? dbid, Building building);
    int AddOrUpdateMove(int? dbid, Move move);
    int AddOrUpdateTenant(int? dbid, Tenant tenant);
    int AddOrUpdateAddress(int? dbid, Address address);
    int AddOrUpdateCountry(int? dbid, Country country);
    int AddOrUpdateUnit(int? dbid, Unit unit);
  }
}
