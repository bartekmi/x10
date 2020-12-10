﻿using System;
using System.Collections.Generic;
using System.Linq;

using Small.Entities;

namespace Small.Repositories {
  public class SmallRepository : ISmallRepository {
    private Dictionary<int, Building> _buildings;
    private Dictionary<int, Tenant> _tenants;
    private Dictionary<int, Move> _moves;

    public SmallRepository() {
      _buildings = CreateBuildings().ToDictionary(t => t.Dbid);
      _tenants = CreateTenants().ToDictionary(t => t.Dbid);
      _moves = CreateMoves().ToDictionary(t => t.Dbid);
    }

    #region Buildings
    public IQueryable<Building> GetBuildings() =>
        _buildings.Values.AsQueryable();


    public Building GetBuilding(int id) {
      return _buildings[id];
    }

    public int AddBuilding(Building building) {
      int newId = _buildings.Values.Max(x => x.Dbid) + 1;
      building.Dbid = newId;
      building.Moniker = "Bldg-" + newId;
      _buildings[newId] = building;
      return newId;
    }

    public void UpdateBuilding(Building building) {
      _buildings[building.Dbid] = building;
    }


    private static IEnumerable<Building> CreateBuildings() {
      return new List<Building>() {
        new Building() {
          Dbid = 1,
          Moniker = "Bldg-1",
          Name = "The Grotto",
          Description = "A stylish bungalow in the middle of town",
          DateOfOccupancy = DateTime.Now,
          MailboxType = MailboxTypeEnum.InBuilding,
          PetPolicy = PetPolicyEnum.DogsOnly,
          MailingAddressSameAsPhysical = true,
          PhysicalAddress = new Address() {
            TheAddress = "111 Prominent Ave",
            City = "Whitehorse",
            StateOrProvince = "YK",
            Zip = "Z57 1Z7",
          },
          Units = new List<Unit>() {
            new Unit() {
              Dbid = 1,
              Number = "Main",
              SquareFeet = 920,
              NumberOfBedrooms = 3,
              NumberOfBathrooms = NumberOfBathroomsEnum.OneAndHalf,
              HasBalcony = true,
            },
            new Unit() {
              Dbid = 2,
              Number = "Basement",
              SquareFeet = 720,
              NumberOfBedrooms = 2,
              NumberOfBathrooms = NumberOfBathroomsEnum.One,
              HasBalcony = false,
            },
          }
        },
        new Building() {
          Dbid = 2,
          Moniker = "Bldg-2",
          Name = "Our Home",
          Description = "Our family home - loved and beautiful",
          DateOfOccupancy = new DateTime(1985, 6, 1),
          MailboxType = MailboxTypeEnum.Community,
          PetPolicy = PetPolicyEnum.NoPets,
          MailingAddressSameAsPhysical = true,
          PhysicalAddress = new Address() {
            TheAddress = "115 Scenic Park Cres NW",
            City = "Calgary",
            StateOrProvince = "AB",
            Zip = "T3L 1R9",
          },
          Units = new List<Unit>(),
        },
      };
    }
    #endregion

    #region Tenants
    public IQueryable<Tenant> GetTenants() =>
        _tenants.Values.AsQueryable();


    public Tenant GetTenant(int id) {
      return _tenants[id];
    }

    public int AddTenant(Tenant tenant) {
      int newId = _tenants.Values.Max(x => x.Dbid) + 1;
      tenant.Dbid = newId;
      _tenants[newId] = tenant;
      return newId;
    }

    public void UpdateTenant(Tenant tenant) {
      _tenants[tenant.Dbid] = tenant;
    }


    private static IEnumerable<Tenant> CreateTenants() {
      return new List<Tenant>() {
        new Tenant() {
          Dbid = 1,
          Name = "Bartek Muszynski",
          Phone = "825-903-2717",
          Email = "220bartek@gmail.com",
          PermanentMailingAddress = new Address() {
            TheAddress = "111 Prominent Ave",
            City = "Whitehorse",
            StateOrProvince = "YK",
            Zip = "Z57 1Z7",
          }
        },
        new Tenant() {
          Dbid = 2,
          Name = "Imelda Muszynski",
          Phone = "825-973-2717",
          Email = "imuszynski@gmail.com",
          PermanentMailingAddress = new Address() {
            TheAddress = "101 Toronto Ave",
            City = "Toronton",
            StateOrProvince = "ON",
            Zip = "Y1Y Z7D",
          }
        },
      };
    }
    #endregion

    #region Moves
    public IQueryable<Move> GetMoves() =>
        _moves.Values.AsQueryable();


    public Move GetMove(int id) {
      return _moves[id];
    }

    private static IEnumerable<Move> CreateMoves() {
      return new List<Move>() {
      };
    }
    #endregion

  }
}
