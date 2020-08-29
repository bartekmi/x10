﻿using System;
using System.Collections.Generic;
using System.Linq;
using wpf_generated.entities;

using wpf_sample.entities.booking;
using wpf_sample.entities.core;

namespace wpf_sample {
  public class DataSourceInMemory : IDataSource {
    private List<Company> _companies;
    public IEnumerable<Company> Companies { get { return _companies; } }

    private List<Location> _locations;
    public IEnumerable<Location> Locations { get { return _locations; } }

    private List<Port> _ports;
    public IEnumerable<Port> Ports { get { return _ports; } }

    public IEnumerable<Booking> Bookings { get; } = new List<Booking>();

    private List<Building> _buildings;
    public IEnumerable<Building> Buildings { get { return _buildings; } }

    public DataSourceInMemory() {
      _companies = new List<Company>() {
        new Company() {
          Id = 1,
          LegalName = "Sapphire Software Inc.",
          Address = "115 Scenic Park Rd. NW",
        },
        new Company() {
          Id = 2,
          LegalName = "Acme, Inc.",
          Address = "179 Acme Rd. NW",
        },
        new Company() {
          Id = 3,
          LegalName = "Safeway Ltd.",
          Address = "1115 Food St. SE",
        },
      };

      _locations = new List<Location>() {
        new Location() {
          Id = 1,
          Name = "Acme Beijing Warehouse",
          City = "Beijing",
          StateOrProvince = "Beijing District",
          CountryCode = "CN",
        },
        new Location() {
          Id = 2,
          Name = "Safeway Story #28",
          City = "Calgary",
          StateOrProvince = "Alberta",
          CountryCode = "CA",
        },
        new Location() {
          Id = 3,
          Name = "Safeway Story #72",
          City = "Edmonton",
          StateOrProvince = "Alberta",
          CountryCode = "CA",
        },
      };

      _ports = new List<Port>() {
        new Port() {
          Id = 1,
          Name = "Port of Guangdong",
          City = "Guangdong",
          CountryCode = "CN",
        },
        new Port() {
          Id = 2,
          Name = "Los Angeles Port Authority",
          City = "Los Angeles",
          StateOrProvince = "CA",
          CountryCode = "US",
        },
        new Port() {
          Id = 3,
          Name = "Seatle Port Authority",
          City = "Seattle",
          StateOrProvince = "WA",
          CountryCode = "US",
        },
      };

      _buildings = new List<Building>() {
        new Building() {
          Id = 1,
          Name = "The District",
          Description = "Fashinable high-rise close to downtown Vancouver",
          PhysicalAddress = new Address() {
            TheAddress = "576 12 Ave",
            City = "Vancouver",
          },
          MailingAddressSameAsPhysical = true,
          DateOfOccupancy = new DateTime(2013, 01, 12),
          PetPolicy = PetPolicyEnum.AllPetsOk,
        },
        new Building() {
          Id = 2,
          Name = "Filipino Community",
          Description = "4-Unit in NW Edmonton",
          PhysicalAddress = new Address() {
            TheAddress = "10609 & 10611 155 Ave",
            City = "Edmonton",
          },
          MailingAddressSameAsPhysical = true,
          DateOfOccupancy = new DateTime(1975, 06, 15),
          PetPolicy = PetPolicyEnum.DogsOnly,
          MailboxType = MailboxTypeEnum.InBuilding,
        },
      };
    }

    public void UpdateOrCreate(Booking booking) {
      ((List<Booking>)Bookings).Add(booking);
    }

    public void UpdateOrCreate(Building building) {
      if (building.IsNew()) {
        building.Id = Buildings.Max(x => x.Id) + 1;
        ((List<Building>)Buildings).Add(building);
      }
    }
  }
}
