using System;
using System.Collections.Generic;
using System.Linq;

using Small.Entities;

namespace Small.Repositories {
  public class SmallRepository : ISmallRepository {
    private Dictionary<int, Building> _buildings;
    private Dictionary<int, Tenant> _tenants;
    private Dictionary<int, Move> _moves;

    public SmallRepository() {
      _buildings = CreateBuildings().ToDictionary(t => t.Id);
      _tenants = CreateTenants().ToDictionary(t => t.Id);
      _moves = CreateMoves().ToDictionary(t => t.Id);
    }

    #region Buildings
    public IQueryable<Building> GetBuildings() =>
        _buildings.Values.AsQueryable();


    public Building GetBuilding(int id) {
      return _buildings[id];
    }

    public int AddBuilding(Building building) {
      int newId = _buildings.Values.Max(x => x.Id) + 1;
      building.Id = newId;
      building.Moniker = "Bldg-" + newId;
      _buildings[newId] = building;
      return newId;
    }


    private static IEnumerable<Building> CreateBuildings() {
      return new List<Building>() {
        new Building() {
          Id = 1,
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
              Number = "Main",
              SquareFeet = 920,
              NumberOfBedrooms = 3,
              NumberOfBathrooms = NumberOfBathroomsEnum.OneAndHalf,
              HasBalcony = true,
            },
            new Unit() {
              Number = "Basement",
              SquareFeet = 720,
              NumberOfBedrooms = 2,
              NumberOfBathrooms = NumberOfBathroomsEnum.One,
              HasBalcony = false,
            },
          }
        },
        new Building() {
          Id = 2,
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
          }
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

    private static IEnumerable<Tenant> CreateTenants() {
      return new List<Tenant>() {
        new Tenant() {
          Id = 1,
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
          Id = 2,
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
