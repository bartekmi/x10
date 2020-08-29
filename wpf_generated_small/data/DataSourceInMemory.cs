using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using wpf_lib.lib;
using wpf_generated.entities;

namespace wpf_generated.data {
  public class DataSourceInMemory : IDataSource {
    #region Properties
    private List<Building> _buildings;
    public IEnumerable<Building> Buildings { get { return _buildings; } }

    private List<Tenant> _tenants;
    public IEnumerable<Tenant> Tenants { get { return _tenants; } }

    private Dictionary<Type, IEnumerable> TypeToCollection() {
      return new Dictionary<Type, IEnumerable>() {
        { typeof(Building), Buildings },
        { typeof(Tenant), Tenants },
        // Add more types here
      };
    }

    #endregion

    #region Initial Data / Constructor
    public DataSourceInMemory() {
      #region Buildings
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
      #endregion

      #region Tenants
      _tenants = new List<Tenant>() {
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
      #endregion

      Validate();
    }

    private void Validate() {
      foreach (var item in TypeToCollection()) {
        string type = item.Key.Name;
        IEnumerable<EntityBase> records = item.Value.Cast<EntityBase>();
        if (records.Any(x => x.IsNew()))
          throw new Exception("Some id's not set for " + type);

        IEnumerable<int> ids = records.Select(x => x.Id);
        if (new HashSet<int>(ids).Count != ids.Count())
          throw new Exception("Some id's are not unique for " + type);
      }
    }
    #endregion

    #region CreateOrUpdate
    public void CreateOrUpdate(Building model) {
      if (model.IsNew()) {
        model.Id = Buildings.Max(x => x.Id) + 1;
        ((List<Building>)Buildings).Add(model);
      }
    }

    public void CreateOrUpdate(Tenant model) {
      if (model.IsNew()) {
        model.Id = Tenants.Max(x => x.Id) + 1;
        ((List<Tenant>)Tenants).Add(model);
      }
    }
    #endregion

    #region GetById
    public T GetById<T>(int id) where T : EntityBase {
      IEnumerable<T> collection = GetCollection<T>();
      return collection.SingleOrDefault(x => x.Id == id);
    }

    private IEnumerable<T> GetCollection<T>() where T : EntityBase {

      if (TypeToCollection().TryGetValue(typeof(T), out IEnumerable collection))
        return (IEnumerable<T>)collection;

      throw new Exception("No collection for Type: " + typeof(T).Name);
    }
    #endregion
  }
}
