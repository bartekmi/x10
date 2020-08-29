using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using wpf_lib.lib;
using wpf_generated.entities;

namespace wpf_generated.data {
  public class DataSourceInMemory : IDataSource {
    private List<Building> _buildings;
    public IEnumerable<Building> Buildings { get { return _buildings; } }

    public DataSourceInMemory() {
      _buildings = new List<Building>() {
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

    public void CreateOrUpdate(Building building) {
      if (building.IsNew()) {
        building.Id = Buildings.Max(x => x.Id) + 1;
        ((List<Building>)Buildings).Add(building);
      }
    }

    public T GetById<T>(int id) where T : EntityBase {
      IEnumerable<T> collection = GetCollection<T>();
      return collection.SingleOrDefault(x => x.Id == id);
    }

    private IEnumerable<T> GetCollection<T>() where T : EntityBase {
      Dictionary<Type, IEnumerable> typeToCollection = new Dictionary<Type, IEnumerable>() {
        { typeof(Building), Buildings },
        // Add more types here
      };

      if (typeToCollection.TryGetValue(typeof(T), out IEnumerable collection))
        return (IEnumerable<T>)collection;

      throw new Exception("No collection for Type: " + typeof(T).Name);
    }

  }
}
