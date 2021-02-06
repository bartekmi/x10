using System;
using System.Collections.Generic;
using System.Linq;

using x10.hotchoc;
using x10.hotchoc.SmallSample.Entities;

namespace x10.hotchoc.SmallSample.Repositories {
  public class Repository : RepositoryBase, IRepository {
    private Dictionary<int, Building> _buildings = new Dictionary<int, Building>();
    private Dictionary<int, Move> _moves = new Dictionary<int, Move>();
    private Dictionary<int, Tenant> _tenants = new Dictionary<int, Tenant>();
    private Dictionary<int, Address> _addresses = new Dictionary<int, Address>();
    private Dictionary<int, Unit> _units = new Dictionary<int, Unit>();

    public override IEnumerable<Type> Types() {
      return new Type[] {
          typeof(Queries),
          typeof(Mutations),
          typeof(Address),
          typeof(Tenant),
          typeof(Unit),
          typeof(Building),
          typeof(Move)
      };
    }

    public override void Add(int id, PrimordialEntityBase instance) {
      if (instance is Building building) _buildings[id] = building;
      if (instance is Move move) _moves[id] = move;
      if (instance is Tenant tenant) _tenants[id] = tenant;
      if (instance is Address address) _addresses[id] = address;
      if (instance is Unit unit) _units[id] = unit;
    }

    #region Buildings
    public IQueryable<Building> GetBuildings() => _buildings.Values.AsQueryable();
    public Building GetBuilding(int id) { return _buildings[id]; }
    public int AddOrUpdateBuilding(int? dbid, Building building) {
      return RepositoryUtils.AddOrUpdate(dbid, building, _buildings);
    }
    #endregion

    #region Moves
    public IQueryable<Move> GetMoves() => _moves.Values.AsQueryable();
    public Move GetMove(int id) { return _moves[id]; }
    public int AddOrUpdateMove(int? dbid, Move move) {
      return RepositoryUtils.AddOrUpdate(dbid, move, _moves);
    }
    #endregion

    #region Tenants
    public IQueryable<Tenant> GetTenants() => _tenants.Values.AsQueryable();
    public Tenant GetTenant(int id) { return _tenants[id]; }
    public int AddOrUpdateTenant(int? dbid, Tenant tenant) {
      return RepositoryUtils.AddOrUpdate(dbid, tenant, _tenants);
    }
    #endregion

    #region Addresses
    public IQueryable<Address> GetAddresses() => _addresses.Values.AsQueryable();
    public Address GetAddress(int id) { return _addresses[id]; }
    public int AddOrUpdateAddress(int? dbid, Address address) {
      return RepositoryUtils.AddOrUpdate(dbid, address, _addresses);
    }
    #endregion

    #region Units
    public IQueryable<Unit> GetUnits() => _units.Values.AsQueryable();
    public Unit GetUnit(int id) { return _units[id]; }
    public int AddOrUpdateUnit(int? dbid, Unit unit) {
      return RepositoryUtils.AddOrUpdate(dbid, unit, _units);
    }
    #endregion

  }
}
