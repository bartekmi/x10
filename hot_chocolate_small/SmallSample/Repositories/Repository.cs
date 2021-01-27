﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;

using x10.hotchoc.Entities;

namespace x10.hotchoc.Repositories {
  public class Repository : IRepository {
    private Dictionary<int, Building> _buildings;
    private Dictionary<int, Tenant> _tenants;
    private Dictionary<int, Move> _moves;

    public Repository() {
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

    public int AddOrUpdateBuilding(int? dbid, Building building) {
      return RepositoryUtils.AddOrUpdate(dbid, building, _buildings);
    }

    private static IEnumerable<Building> CreateBuildings() {
      return new List<Building>() {
        new Building() {
          Dbid = 1,
          Name = "The Grotto",
          Description = "A stylish bungalow in the middle of town",
          DateOfOccupancy = DateTime.Now,
          MailboxType = MailboxTypeEnum.InBuilding,
          PetPolicy = PetPolicyEnum.DogsOnly,
          MailingAddressSameAsPhysical = true,
          PhysicalAddress = new Address() {
            Dbid = 1,
            UnitNumber = "",
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
          Name = "Our Home",
          Description = "Our family home - loved and beautiful",
          DateOfOccupancy = new DateTime(1985, 6, 1),
          MailboxType = MailboxTypeEnum.Community,
          PetPolicy = PetPolicyEnum.NoPets,
          MailingAddressSameAsPhysical = true,
          PhysicalAddress = new Address() {
            Dbid = 2,
            UnitNumber = "",
            TheAddress = "115 Scenic Park Cres NW",
            City = "Calgary",
            StateOrProvince = "AB",
            Zip = "T3L 1R9",
          },
          Units = new List<Unit>(),
        },
        new Building() {
          Dbid = 3,
          Name = "Separate Mailing Address",
          Description = "PO Box mailing address",
          DateOfOccupancy = new DateTime(1985, 6, 1),
          MailboxType = MailboxTypeEnum.Community,
          PetPolicy = PetPolicyEnum.NoPets,
          MailingAddressSameAsPhysical = false,
          PhysicalAddress = new Address() {
            Dbid = 3,
            UnitNumber = "",
            TheAddress = "111 Fair Lane",
            City = "Fancytown",
            StateOrProvince = "AB",
            Zip = "T3Q 1R8",
          },
          MailingAddress = new Address() {
            Dbid = 4,
            UnitNumber = "",
            TheAddress = "PO Box #12345",
            City = "Edmonton",
            StateOrProvince = "AB",
            Zip = "T3R 7V7",
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
      int newId = EnsureUniqueDbids(tenant);
      _tenants[newId] = tenant;
      return newId;
    }

    public void UpdateTenant(Tenant tenant) {
      EnsureUniqueDbids(tenant);
      _tenants[tenant.Dbid] = tenant;
    }

    private static int EnsureUniqueDbids(Tenant tenant) {
      tenant.EnsureUniqueDbid();
      tenant.PermanentMailingAddress.EnsureUniqueDbid();

      return tenant.Dbid;
    }

    private static IEnumerable<Tenant> CreateTenants() {
      return new List<Tenant>() {
        new Tenant() {
          Dbid = 1,
          Name = "Bartek Muszynski",
          Phone = "825-903-2717",
          Email = "220bartek@gmail.com",
          PermanentMailingAddress = new Address() {
            Dbid = 3,
            UnitNumber = "",
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
            Dbid = 4,
            UnitNumber = "",
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

    public int AddMove(Move move) {
      int newId = EnsureUniqueDbids(move);
      _moves[newId] = move;
      return newId;
    }

    public void UpdateMove(Move move) {
      EnsureUniqueDbids(move);
      _moves[move.Dbid] = move;
    }

    private static int EnsureUniqueDbids(Move move) {
      move.EnsureUniqueDbid();
      return move.Dbid;
    }

    private IEnumerable<Move> CreateMoves() {
      return new List<Move>() {
        new Move() {
          Dbid = 1,
          Date = new DateTime(2021, 1, 31),
          From = _buildings[1],
          To = _buildings[2],
          Tenant = _tenants[1]
        },
        new Move() {
          Dbid = 2,
          Date = new DateTime(2021, 2, 15),
          From = _buildings[2],
          To = _buildings[3],
          Tenant = _tenants[2]
        },
      };
    }

    #endregion

  }
}
