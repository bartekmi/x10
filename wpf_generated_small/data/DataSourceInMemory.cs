using System;
using System.Collections.Generic;
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
            City = "Calgary",
            Zip = "T3L 1R9",
          }
        },
      };
    }

    public void Create(Building building) {
      ((List<Building>)Buildings).Add(building);
    }

  }
}
